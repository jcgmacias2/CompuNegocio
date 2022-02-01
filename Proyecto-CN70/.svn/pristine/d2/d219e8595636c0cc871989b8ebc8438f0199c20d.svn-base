using Aprovi.Business.Services;
using Aprovi.Business.ViewModels;
using Aprovi.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Presenters
{
    public class CustomsApplicationsEntrancePresenter
    {
        private ICustomsApplicationsExitView _view;
        private IPedimentoService _customsApplications;
        private IArticuloService _items;
        private Dictionary<int, List<VMPedimentoAsociado>> _cApplications;

        public CustomsApplicationsEntrancePresenter(ICustomsApplicationsExitView view, IPedimentoService customsApplications, IArticuloService items)
        {
            _view = view;
            _customsApplications = customsApplications;
            _items = items;

            _view.Add += Add;
            _view.AutoFill += AutoFill;
            _view.Remove += Remove;
            _view.Save += Save;
            _view.ShowAvailable += ShowAvailable;

            //Se guardan los pedimentos originales
            _cApplications = new Dictionary<int, List<VMPedimentoAsociado>>();
            _view.Items.ForEach(x=>_cApplications.Add(x.Articulo.idArticulo, x.Pedimentos.Select(y => new VMPedimentoAsociado() { Articulo = y.Articulo, Cantidad = y.Cantidad, IdPedimento = y.IdPedimento, NumeroDePedimento = y.NumeroDePedimento }).ToList()));
        }

        private void ShowAvailable()
        {
            try
            {
                var selected = _view.Selected;

                if (!selected.isValid())
                    return;

                //En lugar de todos los pedimentos, solo se muestran los pedimentos incluidos
                var customsApplications = _cApplications[selected.Articulo.idArticulo].Select(x=> new VMPedimentoAsociado(){Articulo = x.Articulo, Cantidad = x.Cantidad, IdPedimento = x.IdPedimento, NumeroDePedimento = x.NumeroDePedimento}).ToList();

                var available = new List<VMPedimentoDisponible>();
                customsApplications.ForEach(ca => available.Add(new VMPedimentoDisponible() { IdPedimento = ca.IdPedimento, NumeroDePedimento = ca.NumeroDePedimento, Existencia = ca.Cantidad, Asociar = ca.Cantidad }));

                //Calcula los asignados
                foreach (var a in available)
                {
                    var item = selected.Pedimentos.FirstOrDefault(y => a.IdPedimento == y.IdPedimento);
                    a.Asociar = item.isValid() ? item.Cantidad : 0m;
                }

                //Muestro los disponibles
                _view.Show(available);

                //Muestra los asociados actualmente
                _view.Show(selected.Pedimentos);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Save()
        {
            try
            {
                //Valido que todos los artículos hayan satisfacido su venta
                if (_view.Items.Any(i => !i.Vendidos.Equals(i.Asociados)))
                    throw new Exception("La cantidad de unidades por artículo asociadas debe ser igual a las vendidas");

                _view.CloseWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Remove()
        {
            try
            {
                var associated = _view.AssociatedSelected;

                if (!associated.isValid())
                    return;

                var item = _view.Selected;
                var availables = _view.Availables;

                availables.FirstOrDefault(a => a.IdPedimento.Equals(associated.IdPedimento)).Asociar = 0.0m;

                item.Pedimentos.Remove(associated);

                _view.Show(availables);
                _view.Show(item);
                
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void AutoFill()
        {
            try
            {
                var availables = _view.Availables;

                if (!_view.Availables.isValid())
                    return;

                var selectedItem = _view.Selected;

                //Paso pedimento por pedimento hasta llenar lo vendido
                var pending = selectedItem.Vendidos;
                int i = 0;
                while (pending > 0.0m || i > availables.Count)
                {
                    //Tomo el pedimento
                    var available = availables[i];

                    //Reviso cuantas unidades asociar, si tiene una existencia mayor a lo pendiente, solo lo pendiente, de lo contrario la existencia entera
                    available.Asociar = available.Existencia > pending ? pending : available.Existencia;
                    //Lo resto de los pendientes
                    pending = pending - available.Asociar;
                    //Avanzo al siguiente
                    i++;
                }

                //Si llegó aqui entonces paso la asociación
                selectedItem.Pedimentos = new List<VMPedimentoAsociado>();
                availables.Where(a => a.Asociar>0.0m).ToList().ForEach(a => selectedItem.Pedimentos.Add(new VMPedimentoAsociado() { Articulo = selectedItem.Articulo, IdPedimento = a.IdPedimento, NumeroDePedimento = a.NumeroDePedimento, Cantidad = a.Asociar }));

                _view.Show(availables);
                _view.Show(selectedItem);

            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Add()
        {
            try
            {
                if (!_view.Availables.isValid())
                    return;

                //Obtengo aquellos que tienen una cantidad a ser asociada
                var available = _view.Availables.Where(a => a.Asociar > 0.0m).ToList();

                //Si no registró asociaciones, me regreso
                if (!available.isValid() || available.Count().Equals(0))
                    return;

                //Valido que no haya asociado más de lo existente
                //available.ForEach(a => a.Asociar = a.Asociar > a.Existencia ? a.Existencia : a.Asociar);
                
                var selectedItem = _view.Selected;

                //Valido que no haya asociado más de lo requerido
                if (available.Sum(a => a.Asociar) > selectedItem.Vendidos)
                    throw new Exception("La cantidad de unidades asociada es mayor a la vendida");

                //Si llegó aqui entonces paso la asociación
                selectedItem.Pedimentos = new List<VMPedimentoAsociado>();
                available.ForEach(a => selectedItem.Pedimentos.Add(new VMPedimentoAsociado() { Articulo = selectedItem.Articulo, IdPedimento = a.IdPedimento, NumeroDePedimento = a.NumeroDePedimento, Cantidad = a.Asociar }));

                _view.Show(selectedItem);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
