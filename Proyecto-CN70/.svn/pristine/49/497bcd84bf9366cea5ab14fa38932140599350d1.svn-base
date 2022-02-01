using Aprovi.Business.Services;
using Aprovi.Views;
using System;

namespace Aprovi.Presenters
{
    public class ItemsHomologationToolPresenter
    {
        private IItemsHomologationToolView _view;
        private IMigrationDataService _excelData;

        public ItemsHomologationToolPresenter(IItemsHomologationToolView view, IMigrationDataService excelData)
        {
            _view = view;
            _excelData = excelData;

            _view.Quit += Quit;
            _view.ProcessItems += ProcessItems;
            _view.OpenFindXls += OpenFindXls;
        }

        private void OpenFindXls()
        {
            try
            {
                var excelPath = _view.OpenFileFinder("Homologación en excel (*.xls, *.xlsx)|*.xls; *.xlsx");

                if (!excelPath.isValid())
                {
                    _view.ShowMessage("Debe seleccionar un archivo de excel de donde obtener los registros de homologación");
                    return;
                }

                _view.Fill(excelPath);
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

        private void ProcessItems()
        {
            try
            {
                if (!_view.ExcelFile.isValid())
                    throw new Exception("Debe seleccionar el archivo de excel con la homologación");

                if (!_view.CNStartCell.isValid())
                    throw new Exception("Debe especificar la primer celda que contiene el código de CompuNegocio");

                if (!_view.SATStartCell.isValid())
                    throw new Exception("Debe especificar la primera celda que contiene el código del SAT");

                var records = _excelData.Homologate(_view.ExcelFile, _view.CNStartCell, _view.SATStartCell);

                _view.ShowMessage("{0} registro homologados exitosamente", records);

                _view.CloseWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
