using Aprovi.Data.Core;
using Aprovi.Data.Repositories;
using Aprovi.Business.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;
using Aprovi.Business.Helpers;
using Aprovi.Data.Models;

namespace Aprovi.Business.Services
{
    public abstract class TraspasoService : ITraspasoService
    {
        private IUnitOfWork _UOW;
        private ITraspasosRepository _transfers;
        private IConfiguracionService _config;
        private IArticulosRepository _items;
        private IPedimentoService _customsApprovals;
        
        public TraspasoService(IUnitOfWork unitOfWork,  IConfiguracionService config, IPedimentoService customsApprovals)
        {
            _UOW = unitOfWork;
            _transfers = _UOW.Traspasos;
            _config = config;
            _items = _UOW.Articulos;
            _customsApprovals = customsApprovals;
        }

        public int Next()
        {
            try
            {
                return _transfers.Next();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Last()
        {
            try
            {
                return _transfers.Last();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual Traspaso Add(Traspaso transfer)
        {
            try
            {
                //Obtengo una instancia de la configuración
                var config = _config.GetDefault();

                //Antes de registrarla obtengo nuevamente el folio, por si acaso ya se utilizo mientras agregaba los artículos
                transfer.folio = _transfers.Next();

                //Le agrego estado
                transfer.idEstatusDeTraspaso = (int)StatusDeTraspaso.Registrado;

                //Defaults de traspaso
                transfer.EmpresasAsociada = null;
                transfer.EmpresasAsociada1 = null;

                //Esto ahora se maneja en la vista
                //transfer.fechaHora = DateTime.Now;

                //Ahora si guardo
                _transfers.Add(transfer);
                _UOW.Save();

                return transfer;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Traspaso FindById(int idTransfer)
        {
            try
            {
                //Este debe pasarse como objecto
                return _transfers.FindById(idTransfer);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Traspaso FindByIdForTransfer(int idTransfer)
        {
            try
            {
                //Este debe pasarse como objecto
                return _transfers.FindByIdForTransfer(idTransfer);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Traspaso FindByFolio(int folio)
        {
            try
            {
                return _transfers.Find(folio);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Traspaso> List()
        {
            try
            {
                return _transfers.List().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Traspaso> WithFolioOrCompanyLike(string value)
        {
            try
            {
                return _transfers.WithFolioOrCompanyLike(value, null);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Traspaso Reject(VMTraspaso transfer)
        {
            try
            {
                var dbTransfer = _transfers.Find(transfer.TransferRequest.folio);

                //No puedo cancelar un traspaso ya procesado
                if (transfer.idEstatusDeTraspaso != (int)StatusDeTraspaso.Registrado)
                    throw new Exception("No es posible rechazar un traspaso procesado");

                dbTransfer.idEstatusDeTraspaso = (int)StatusDeTraspaso.Rechazado;

                _transfers.Update(dbTransfer);
                _UOW.Save();

                return transfer;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Traspaso Update(Traspaso transfer, List<DetallesDeTraspaso> detail)
        {
            try
            {
                //Se obtiene el pedido original
                var transferDb = _transfers.FindById(transfer.idTraspaso);

                if (transferDb.idEstatusDeTraspaso != (int) StatusDeTraspaso.Registrado)
                {
                    throw new Exception("No se pueden modificar traspasos procesados");
                }

                //Se determina si hubo cambios en el detalle
                var dbDetail = transferDb.DetallesDeTraspasoes.ToList();
                List<DetallesDeTraspaso> removedItems = new List<DetallesDeTraspaso>();
                List<DetallesDeTraspaso> addedItems = new List<DetallesDeTraspaso>();
                foreach (var d in dbDetail)
                {
                    var detailItem = detail.FirstOrDefault(x=>x.idArticulo == d.idArticulo);

                    if (!detailItem.isValid())
                    {
                        //Se elimino el articulo del detalle
                        removedItems.Add(d);
                        continue;
                    }
                    else
                    {
                        //Se actualiza el articulo
                        d.cantidadEnviada = detailItem.cantidadEnviada;
                        d.cantidadAceptada = detailItem.cantidadAceptada;
                    }
                }

                foreach (var d in detail)
                {
                    var item = dbDetail.FirstOrDefault(x=>x.idArticulo == d.idArticulo);
                    //Se buscan detalles nuevos

                    if (!item.isValid())
                    {
                        //Se agrego el articulo al detalle
                        addedItems.Add(d);
                    }
                }

                //Ahora si se pueden efectuar los cambios a la entidad de EF
                removedItems.ForEach(x => _transfers.DeleteDetail(x));
                addedItems.ForEach(x => transferDb.DetallesDeTraspasoes.Add(x));
                transferDb.tipoDeCambio = transfer.tipoDeCambio;

                _UOW.Save();

                return transferDb;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Traspaso Update(Traspaso transfer)
        {
            try
            {
                _transfers.Update(transfer);
                _UOW.Save();

                return transfer;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteDetail(DetallesDeTraspaso transferDetail)
        {
            try
            {
                _transfers.DeleteDetail(transferDetail);
                _UOW.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Traspaso Approve(Traspaso remoteTransfer)
        {
            try
            {
                //Se verifica si el traspaso cuenta con algun articulo importado
                var importedItems = remoteTransfer.DetallesDeTraspasoes.Where(x => !x.PedimentoPorDetalleDeTraspasoes.IsEmpty()).ToList();
                if (!importedItems.IsEmpty())
                {
                    //Se deben registrar todos los pedimentos que no esten registrados
                    
                    //se seleccionan solo los pedimentos de todos los articulos importados
                    var customsApprovals = importedItems.SelectMany(x=>x.PedimentoPorDetalleDeTraspasoes.Select(y=>y.Pedimento)).ToList();

                    //Por cada pedimento, verifico que se encuentre registrado en la base de datos local
                    foreach (var customsApproval in customsApprovals)
                    {
                        var c = _customsApprovals.FindByDetails(customsApproval);

                        //Si no esta registrado, se agrega
                        if (!c.isValid())
                        {
                            //Posiblemente a entity no le guste usar una entidad de otro contexto, asi que se crea una nueva
                            Pedimento p = new Pedimento()
                            {
                                aduana = customsApproval.aduana,
                                añoEnCurso = customsApproval.añoEnCurso,
                                añoOperacion = customsApproval.añoOperacion,
                                patente = customsApproval.patente,
                                progresivo = customsApproval.progresivo,
                                fecha = customsApproval.fecha
                            };

                            _customsApprovals.Add(p);
                        }
                    }
                }

                //Ahora si se puede registrar el traspaso
                Traspaso transfer = new Traspaso()
                {
                    descripcion = remoteTransfer.descripcion,
                    folio = remoteTransfer.folio,
                    fechaHora = DateTime.Now,
                    idEmpresaAsociadaDestino = remoteTransfer.idEmpresaAsociadaDestino,
                    idEmpresaAsociadaOrigen = remoteTransfer.idEmpresaAsociadaOrigen,
                    idEstatusDeTraspaso = remoteTransfer.DetallesDeTraspasoes.Any(x=>x.cantidadEnviada != x.cantidadAceptada) ? (int)StatusDeTraspaso.Parcial : (int)StatusDeTraspaso.Total,
                    idUsuarioRegistro = remoteTransfer.idUsuarioRegistro,
                    tipoDeCambio = remoteTransfer.tipoDeCambio,
                };

                transfer = _transfers.Add(transfer);
                _UOW.Save();

                transfer.DetallesDeTraspasoes = new List<DetallesDeTraspaso>();
                //Se registra el detalle
                foreach (var rtd in remoteTransfer.DetallesDeTraspasoes)
                {
                    //Primero se debe registrar el detalle en la base de datos local
                    //Se deben agrupar los pedimentosPorDetalleDeTraspasos por pedimento
                    var customsDetail = new List<PedimentoPorDetalleDeTraspaso>();
                    var groupedItems = rtd.PedimentoPorDetalleDeTraspasoes.Select(x=>
                    {
                        var pedimento = _customsApprovals.FindByDetails(x.Pedimento);
                        return new PedimentoPorDetalleDeTraspaso()
                        {
                                cantidad = x.cantidad,
                                idPedimento = pedimento.idPedimento,
                                Pedimento = pedimento
                            };
                    }).GroupBy(x => x.idPedimento);

                    foreach (var item in groupedItems)
                    {
                        var i = item.FirstOrDefault();
                        customsDetail.Add(new PedimentoPorDetalleDeTraspaso(){cantidad = item.Sum(x=>x.cantidad), idPedimento = i.idPedimento, Pedimento = i.Pedimento});
                    }

                    DetallesDeTraspaso dt = new DetallesDeTraspaso()
                    {
                        cantidadAceptada = rtd.cantidadAceptada.GetValueOrDefault(0m),
                        cantidadEnviada = rtd.cantidadEnviada,
                        costoUnitario = rtd.costoUnitario,
                        //El id del articulo puede ser diferente entre las bases de datos
                        idArticulo = _items.Find(rtd.Articulo.codigo).idArticulo,
                        idMoneda = rtd.idMoneda,
                        //Ya se registraron todos los pedimentos anteriormente, este paso no deberia tirar ningun error
                        PedimentoPorDetalleDeTraspasoes = customsDetail
                    };
                    transfer.DetallesDeTraspasoes.Add(dt);
                }

                _transfers.Update(transfer);
                _UOW.Save();

                //En este punto el traspaso ya deberia estar registrado el traspaso, con su detalle y los pedimentos

                return transfer;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Traspaso EndRemoteIntegration(Traspaso localTransfer, Traspaso remoteTransfer)
        {
            try
            {
                //Se actualiza el folio y fechahora remotos
                localTransfer.folioRemoto = remoteTransfer.folio;
                localTransfer.fechaHoraRemoto = remoteTransfer.fechaHora;

                var dbTransfer = _transfers.Update(localTransfer);
                _UOW.Save();

                return dbTransfer;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Traspaso  EndLocalIntegration(Traspaso remoteTransfer, Traspaso localTransfer)
        {
            try
            {
                //Se actualiza el folio y fechahora remotos
                remoteTransfer.folioRemoto = localTransfer.folio;
                remoteTransfer.fechaHoraRemoto = localTransfer.fechaHora;

                //Se actualiza la cantidad aceptados
                foreach (var rtd in remoteTransfer.DetallesDeTraspasoes)
                {
                    var localItem = localTransfer.DetallesDeTraspasoes.FirstOrDefault(x => x.idArticulo == rtd.idArticulo);
                    rtd.cantidadAceptada = localItem.cantidadAceptada.GetValueOrDefault(0m);

                    //Se deben actualizar los pedimentos con las cantidades aceptadas
                    var customsApplications = rtd.PedimentoPorDetalleDeTraspasoes.ToList();

                    foreach (var ca in customsApplications)
                    {
                        var updatedApplication = localItem.PedimentoPorDetalleDeTraspasoes.FirstOrDefault(x=>x.idPedimento == ca.idPedimento);

                        if (updatedApplication.isValid())
                        {
                            ca.cantidad = updatedApplication.cantidad;
                        }
                        else
                        {
                            ca.cantidad = 0;
                        }
                    }
                }

                //Se actualiza el estado de la transferencia remota
                remoteTransfer.idEstatusDeTraspaso = remoteTransfer.DetallesDeTraspasoes.Any(x => x.cantidadAceptada != x.cantidadEnviada) ? (int)StatusDeTraspaso.Parcial : (int)StatusDeTraspaso.Total;

                var dbTransfer = _transfers.Update(remoteTransfer);

                _UOW.Save();

                return dbTransfer;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VMRTraspaso> ListForReport(DateTime startDate, DateTime endDate, EmpresasAsociada originCompany,
            EmpresasAsociada destinationCompany)
        {
            try
            {
                return _transfers.List(startDate, endDate, originCompany, destinationCompany).Select(x=> new VMRTraspaso(new VMTraspaso(x))).ToList();
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
