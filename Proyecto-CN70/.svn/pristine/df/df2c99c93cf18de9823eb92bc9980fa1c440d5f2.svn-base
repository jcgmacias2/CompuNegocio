using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Views;
using Aprovi.Views.UI;
using System;

namespace Aprovi.Presenters
{
    public class BanksPresenter
    {
        private IBanksView _view;
        private IBancoService _banks;

        public BanksPresenter(IBanksView view, IBancoService banks)
        {
            _view = view;
            _banks = banks;

            _view.Find += Find;
            _view.OpenList += OpenList;
            _view.New += New;
            _view.Save += Save;
            _view.Delete += Delete;
            _view.Quit += Quit;
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

        private void Delete()
        {
            try
            {
                if (!_banks.CanDelete(_view.Bank.idBanco))
                    throw new Exception("Este banco tiene relaciones existentes por lo que no puede eliminarse");

                _banks.Delete(_view.Bank);

                _view.ShowMessage("Banco eliminado exitosamente");
                _view.Clear();

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
                _banks.Add(_view.Bank);
                _view.ShowMessage("Banco registrado exitosamente");
                _view.Clear();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void New()
        {
            try
            {
                _view.Clear();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenList()
        {
            try
            {
                IBanksListView view;
                BanksListPresenter presenter;

                view = new BanksListView();
                presenter = new BanksListPresenter(view, _banks);

                view.ShowWindow();

                if (view.Bank.isValid() && view.Bank.idBanco.isValid())
                    _view.Show(view.Bank);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Find()
        {
            try
            {
                if (!_view.Bank.nombre.isValid())
                    return;

                var banco = _banks.Find(_view.Bank.nombre);
                if (banco.isValid() && banco.idBanco.isValid())
                    _view.Show(banco);
                else
                    _view.Show(new Banco() { nombre = _view.Bank.nombre });
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }   
        }
    }
}
