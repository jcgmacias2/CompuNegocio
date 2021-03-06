using Aprovi;
using Aprovi.Application.Helpers;
using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Presenters;
using Aprovi.Views;
using Aprovi.Views.UI;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace CompuNegocio
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //Check it the system is already running
            if (IsRunning())
            {
                MessageBox.Show("El sistema ya se encuentra abierto", "Aprovi", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Shutdown();
            }

            //Startup point
            AproviContainer aproviContainer = new AproviContainer();

            //Global App Configuration
            var configurationService = aproviContainer.Container.Resolve<IConfiguracionService>();

            try
            {
                Session.Configuration = configurationService.GetDefault();
            }
            catch (EntityException)
            {
                //Si cae en esta excepción le abro la ventana de configuración del servidor
                IConnectionUpdateView view;
                ConnectionUpdatePresenter connectionPresenter;

                view = new ConnectionUpdateView();
                connectionPresenter = new ConnectionUpdatePresenter(view, aproviContainer.Container.Resolve<IConfiguracionService>());

                view.ShowWindow();

                //Vuelvo a intentar
                Session.Configuration = configurationService.GetDefault();
            }

            try
            {
                //Si llega aqui esto ya esta seguro
                var catalogos = aproviContainer.Container.Resolve<ICatalogosEstaticosService>();
                var comprobante = new TiposDeComprobante();
                //Serie de facturas
                comprobante = catalogos.ListTiposDeComprobante().FirstOrDefault(t => t.idTipoDeComprobante.Equals((int)TipoDeComprobante.Factura));
                if(comprobante.isValid() && comprobante.idSerie.HasValue)
                    Session.SerieFacturas = comprobante.Series;
                //Serie de Abonos y Pagos
                comprobante = catalogos.ListTiposDeComprobante().FirstOrDefault(t => t.idTipoDeComprobante.Equals((int)TipoDeComprobante.Parcialidad));
                if(comprobante.isValid() && comprobante.idSerie.HasValue)
                    Session.SerieParcialidades = comprobante.Series;
                //Serie de Nota de crédito
                comprobante = catalogos.ListTiposDeComprobante().FirstOrDefault(t => t.idTipoDeComprobante.Equals((int)TipoDeComprobante.Nota_De_Credito));
                if(comprobante.isValid() && comprobante.idSerie.HasValue)
                    Session.SerieNotasDeCredito = comprobante.Series;

                //Cargo en configuración los módulos y el código del sistema
                //Me aseguro de tener localmente la información descargada respecto a folios
                if (!Session.Configuration.Mode.Equals(Ambiente.Configuration) && Session.Configuration.Estacion.isValid() && Session.Configuration.Estacion.Empresa.isValid() && Session.Configuration.Estacion.Empresa.licencia.isValid())
                {
                    Session.Configuration = configurationService.UpdateSettings(Session.Configuration.Estacion.Empresa.licencia);
                    //Cargo las personalizaciones en caso de que existan
                    aproviContainer.LoadCustomization(Session.Configuration.Sistema);
                }

                //Cuando esta en modo configuración le permito ver las herramientas de migración
                var menu = new MainView(Session.Configuration.razonSocial);
                if (!Session.Configuration.Mode.Equals(Ambiente.Configuration))
                    menu.mniHerramientasMenu.Visibility = Visibility.Collapsed;

                MainPresenter presenter = new MainPresenter(menu, aproviContainer.Container);
                menu.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private bool IsRunning()
        {
            try
            {
                string procName = Process.GetCurrentProcess().ProcessName;
                string procFile = Process.GetCurrentProcess().MainWindowTitle;

                //Get the list of all the processes by the "procName"     
                IEnumerable<Process> processes = Process.GetProcessesByName(procName);

                var instances = processes.Where(p => p.MainWindowTitle.Equals(procFile));

                return instances.Count() > 1;
            }
            catch (Exception)
            {
                throw;
            }
        }


        #region Global event

        protected override void OnStartup(StartupEventArgs e)
        {
            EventManager.RegisterClassHandler(typeof(TextBox),
                TextBox.GotFocusEvent,
                new RoutedEventHandler(TextBox_GotFocus));

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            App.Current.Shutdown();
            base.OnExit(e);
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).SelectAll();
        }

        #endregion
    }
}
