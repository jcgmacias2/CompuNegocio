using Aprovi.Business.ViewModels;
using Aprovi.Data.Core;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aprovi.Business.Services
{
    public abstract class ListasDePreciosService : IListasDePreciosService
    {
        private IUnitOfWork _UOW;
        private IViewReporteListaDePreciosRepository _priceLists;
        private IViewReporteListaDePreciosConImpuestosRepository _priceListsWithTaxes;
        private IViewReporteListaDePreciosPorListaRepository _priceListForList;
        private IViewReporteListaDePreciosPorListaConImpuestosRepository _priceListForListWithTaxes;

        public ListasDePreciosService(IUnitOfWork unitOfWork)
        {
            _UOW = unitOfWork;
            _priceListForList = unitOfWork.ListaDePreciosPorLista;
            _priceListForListWithTaxes = unitOfWork.ListaDePreciosPorListaConImpuestos;
            _priceLists = unitOfWork.ListaDePrecios;
            _priceListsWithTaxes = unitOfWork.ListaDePreciosConImpuestos;
        }


        public VMRListaDePrecios List(ListasDePrecio priceList, Clasificacione classification, bool onlyWithStock, bool includeNonStocked, bool includeTaxes, Moneda currency,
            decimal exchangeRate)
        {
            try
            {
                List<VMRDetalleListaDePrecios> detail = new List<VMRDetalleListaDePrecios>();

                if (priceList.isValid() && priceList.idListaDePrecio.isValid() && !includeTaxes)
                {
                    //Lista de precio sin impuestos
                    detail = _priceListForList.WithClassification(priceList, classification, onlyWithStock, includeNonStocked).Select(x=> new VMRDetalleListaDePrecios(x)).ToList();
                }

                if (priceList.isValid() && priceList.idListaDePrecio.isValid() && includeTaxes)
                {
                    //Lista de precio con impuestos
                    detail = _priceListForListWithTaxes.WithClassification(priceList, classification, onlyWithStock, includeNonStocked).Select(x => new VMRDetalleListaDePrecios(x)).ToList();
                }

                if((!priceList.isValid() || !priceList.idListaDePrecio.isValid() && !includeTaxes))
                {
                    //Todas las listas de precio sin impuestos
                    detail = _priceLists.WithClassification(classification, onlyWithStock, includeNonStocked).Select(x => new VMRDetalleListaDePrecios(x)).ToList();
                }

                if ((!priceList.isValid() || !priceList.idListaDePrecio.isValid() && includeTaxes))
                {
                    //Todas las listas de precio con impuestos
                    detail = _priceListsWithTaxes.WithClassification(classification, onlyWithStock, includeNonStocked).Select(x => new VMRDetalleListaDePrecios(x)).ToList();
                }

                //Se efectua la conversion de moneda
                foreach (var i in detail)
                {
                    i.PrecioA = i.PrecioA.ToDocumentCurrency(new Moneda() { idMoneda = i.IdMoneda }, currency, exchangeRate);

                    if (!priceList.isValid() || !priceList.idListaDePrecio.isValid())
                    {
                        //Se calculan los demas precios
                        i.PrecioB = i.PrecioB.ToDocumentCurrency(new Moneda() { idMoneda = i.IdMoneda }, currency, exchangeRate);
                        i.PrecioC = i.PrecioC.ToDocumentCurrency(new Moneda() { idMoneda = i.IdMoneda }, currency, exchangeRate);
                        i.PrecioD = i.PrecioD.ToDocumentCurrency(new Moneda() { idMoneda = i.IdMoneda }, currency, exchangeRate);
                    }
                }

                return new VMRListaDePrecios(){Clasificacion = classification, Detalle = detail, IncluirImpuestos = includeTaxes, Moneda = currency, TipoDeCambio = exchangeRate, IncluirNoInventariados = includeNonStocked, SoloConInventario = onlyWithStock};
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
