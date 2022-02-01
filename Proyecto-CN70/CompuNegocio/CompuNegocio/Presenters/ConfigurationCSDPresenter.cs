using Aprovi.Business.Services;
using Aprovi.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Presenters
{
    public class ConfigurationCSDPresenter
    {
        private IConfigurationCSDView _view;
        private ICertificadoService _certificates;

        public ConfigurationCSDPresenter(IConfigurationCSDView view, ICertificadoService certificateService)
        {
            _view = view;
            _certificates = certificateService;

            _view.OpenFindCertificate += OpenFindCertificate;
            _view.OpenFindPrivateKey += OpenFindPrivateKey;
            _view.OpenPfxFolder += OpenPfxFolder;
            _view.OpenFindPfx += OpenFindPfx;

            _view.Quit += Quit;
            _view.Save += Save;
            _view.CreateAndSave += CreateAndSave;
        }

        private void OpenFindPfx()
        {
            try
            {
                var certificate = _view.Certificate;

                certificate.CertificadoPFX = _view.OpenFileFinder("Certificado CSD (*.pfx)|*.pfx");

                _view.Show(certificate);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenPfxFolder()
        {
            try
            {
                var certificate = _view.Certificate;

                certificate.FolderPfx = _view.OpenFolderFinder("Seleccione el folder donde dejar el CSD");

                _view.Show(certificate);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenFindPrivateKey()
        {
            try
            {
                var certificate = _view.Certificate;

                certificate.LlavePrivadaKEY = _view.OpenFileFinder("Llave Privada (*.key)|*.key");

                _view.Show(certificate);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenFindCertificate()
        {
            try
            {
                var certificate = _view.Certificate;

                certificate.CertificadoCER = _view.OpenFileFinder("Certificado de Sello Digital (*.cer)|*.cer");

                _view.Show(certificate);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void CreateAndSave()
        {
            //Debo validar que tengo toda la información
            var certificate = _view.Certificate;

            if(!certificate.CertificadoCER.isValid())
            {
                _view.ShowError("Es necesario seleccionar un certificado (*.cer) válido");
                return;
            }

            if(!certificate.LlavePrivadaKEY.isValid())
            {
                _view.ShowError("Es necesario seleccionar una llave privada (*.key) válida");
                return;
            }

            if(!certificate.ContraseñaLlavePrivada.isValid())
            {
                _view.ShowError("Es necesario capturar la contraseña de la llave privada");
                return;
            }

            if(!certificate.FolderPfx.isValid())
            {
                _view.ShowError("Debe especificar la ruta en donde se depositará el archivo pfx del csd");
                return;
            }

            try
            {                
                //Intentar la configuración
                _certificates.Configure(certificate.CertificadoCER, certificate.LlavePrivadaKEY, certificate.ContraseñaLlavePrivada, certificate.FolderPfx);

                //Envío mensaje de éxito
                _view.ShowMessage("Certificado configurado exitosamente");

                //Lo lleno en la vista
                _view.Show(certificate);
                
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Save()
        {
            //Debo validar que tengo toda la información
            var certificate = _view.Certificate;

            if (!certificate.CertificadoPFX.isValid())
            {
                _view.ShowError("Es necesario seleccionar un pfx existente");
                return;
            }

            try
            {
                //Intentar la configuración
                _certificates.Configure(certificate.CertificadoPFX);

                //Envío mensaje de éxito
                _view.ShowMessage("Certificado configurado exitosamente");

                //Lo lleno en la vista
                _view.Show(certificate);
                
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Quit()
        {
            try
            {
                _view.CloseWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
