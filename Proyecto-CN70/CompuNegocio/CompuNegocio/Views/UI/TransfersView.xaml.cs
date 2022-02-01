using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using Aprovi.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.ObjectBuilder2;

namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for TransfersView.xaml
    /// </summary>
    public partial class TransfersView : BaseView, ITransfersView
    {
        public event Action Find;
        public event Action OpenList;
        public event Action Load;
        public event Action FindItem;
        public event Action OpenItemsList;
        public event Action AddItem;
        public event Action RemoveItem;
        public event Action SelectItem;
        public event Action Quit;
        public event Action Reject;
        public event Action New;
        public event Action Print;
        public event Action Save;
        public event Action Update;
        public event Action Approve;
        public event Action OpenDestinationAssociatedCompanyList;
        public event Action FindDestinationAssociatedCompany;
        public event Action LoadRemoteTransfer;

        private int _idTraspaso;
        private bool _isLocal;
        private EmpresasAsociada _sourceCompany;
        private EmpresasAsociada _destinationCompany;
        private VMTraspaso _transfer;
        private DetallesDeTraspaso _currentItem;
        private SolicitudesDeTraspaso _transferRequest;

        public TransfersView(SolicitudesDeTraspaso transferRequest)
        {
            InitializeComponent();
            BindComponents();
            _transferRequest = transferRequest;
        }

        public TransfersView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            this.Loaded += TransfersView_Loaded;

            txtFolio.LostFocus += txtFolio_LostFocus;
            btnListarTraspasos.Click += BtnListarTraspasosOnClick;
            txtEmpresaAsociadaDestino.LostFocus += txtEmpresaAsociadaDestino_LostFocus;
            btnListarEmpresaAsociadaDestino.Click += btnListarEmpresaAsociadaDestino_Click;

            txtArticuloCodigo.LostFocus += txtArticuloCodigo_LostFocus;
            txtArticuloCodigo.PreviewKeyDown += TxtArticuloCodigo_PreviewKeyDown;
            btnListarArticulos.Click += btnListarArticulos_Click;
            txtArticuloCantidadEnviados.GotFocus += txtArticuloCantidad_GotFocus;
            txtArticuloCosto.PreviewKeyDown += txtArticuloCosto_PreviewKeyDown;

            dgDetalle.PreviewKeyUp += dgDetalle_PreviewKeyUp;
            dgDetalle.MouseDoubleClick += dgDetalle_MouseDoubleClick;

            btnCerrar.Click += btnCerrar_Click;
            btnNuevo.Click += btnNuevo_Click;
            btnRechazar.Click += btnRechazar_Click;
            btnImprimir.Click += btnImprimir_Click;
            btnRegistrar.Click += btnRegistrar_Click;

            _sourceCompany = new EmpresasAsociada();
            _destinationCompany = new EmpresasAsociada();
            _transfer = new VMTraspaso();
            _currentItem = new DetallesDeTraspaso(){Articulo = new Articulo()};
            _transferRequest = new SolicitudesDeTraspaso();
        }

        private void BtnListarTraspasosOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (OpenList.isValid())
            {
                OpenList();
            }
        }

        private void btnListarEmpresaAsociadaDestino_Click(object sender, RoutedEventArgs e)
        {
            if (OpenDestinationAssociatedCompanyList.isValid())
            {
                OpenDestinationAssociatedCompanyList();
            }
        }

        private void txtEmpresaAsociadaDestino_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FindDestinationAssociatedCompany.isValid())
            {
                FindDestinationAssociatedCompany();
            }
        }

        void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            if (!IsDirty)
            {
                if (Save.isValid(AccesoRequerido.Ver_y_Agregar))
                    Save();
            }
            else
            {
                if (!IsBeingProcessed)
                {
                    if (Update.isValid(AccesoRequerido.Total))
                    {
                        Update();
                    }
                }
                else
                {
                    if (Approve.isValid(AccesoRequerido.Total))
                    {
                        Approve();
                    }
                }
            }
        }

        void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            if (Print.isValid(AccesoRequerido.Ver))
                Print();
        }

        void btnRechazar_Click(object sender, RoutedEventArgs e)
        {
            if (Reject.isValid(AccesoRequerido.Total))
                Reject();
        }

        void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            if (New.isValid())
                New();
        }

        void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        void dgDetalle_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SelectItem.isValid())
                SelectItem();
        }

        void dgDetalle_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Delete) && RemoveItem.isValid(AccesoRequerido.Ver_y_Agregar) && !IsBeingProcessed)
                RemoveItem();
        }

        void txtArticuloCosto_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && AddItem.isValid())
            {
                e.Handled = true;
                AddItem();
                txtArticuloCodigo.Focus();
            }
        }

        void txtArticuloCantidad_GotFocus(object sender, RoutedEventArgs e)
        {
            //Al perder el foco del txtArticuloCodigo llega aquí
            //Si el articulo no existe enfoco nuevamente código de articulo
            if (!txtArticuloCantidadEnviados.Text.isValid())
            {
                txtArticuloCodigo.Focus();
                return;
            }
        }

        void btnListarArticulos_Click(object sender, RoutedEventArgs e)
        {
            if (OpenItemsList.isValid())
                OpenItemsList();
        }

        private void TxtArticuloCodigo_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.F2) && OpenItemsList.isValid())
                OpenItemsList();
        }

        void txtArticuloCodigo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FindItem.isValid())
                FindItem();
        }

        void txtFolio_LostFocus(object sender, RoutedEventArgs e)
        {
            if (SelectedTransferChanged && Find.isValid())
                Find();
        }

        void TransfersView_Loaded(object sender, RoutedEventArgs e)
        {
            //Si la transaccion se inicio con una solicitud de traspaso, se debe cargar el traspaso desde la base de datos origen
            if (!IsBeingProcessed)
            {
                if (Load.isValid())
                    Load();
            }
            else
            {
                if (LoadRemoteTransfer.isValid())
                {
                    LoadRemoteTransfer();
                }
            }

        }

        public VMTraspaso Transfer
        {
            get
            {
                return IsDirty && !SelectedTransferChanged
                    ? _transfer
                    : new VMTraspaso() //Cuando es un traspaso ya registrado, regreso el registro completo que se cargo
                    {
                        descripcion = txtDescripcion.Text,
                        idEmpresaAsociadaOrigen = _sourceCompany.idEmpresaAsociada,
                        idEmpresaAsociadaDestino = _destinationCompany.idEmpresaAsociada,
                        folio = txtFolio.Text.ToIntOrDefault(),
                        idTraspaso = _idTraspaso,
                        EmpresasAsociada = new EmpresasAsociada(){ nombre = txtEmpresaAsociadaDestino.Text , idEmpresaAsociada = _destinationCompany.idEmpresaAsociada },
                        EmpresasAsociada1 = _sourceCompany,
                        Detalle = dgDetalle.Items.Cast<DetallesDeTraspaso>().ToList(),
                        tipoDeCambio = txtTipoDeCambio.Text.ToDecimalOrDefault(),
                        TransferRequest = _transferRequest,
                        fechaHora = dpFecha.SelectedDate.GetValueOrDefault(DateTime.Today)
                    };
            }
        }

        public bool IsDirty
        {
            get { return _idTraspaso.isValid(); }
        }

        private bool SelectedTransferChanged => (!_transfer.isValid() || txtFolio.Text.isValid() && _transfer.folio != txtFolio.Text.ToIntOrDefault());

        public bool IsBeingProcessed { get { return _transferRequest.isValid() && _transferRequest.folio.isValid();} }

        public DetallesDeTraspaso CurrentItem
        {
            get
            {
                return new DetallesDeTraspaso()
                {
                    cantidadAceptada = txtArticuloCantidadAceptados.Text.ToValidatedDecimal(),
                    idTraspaso = _idTraspaso,
                    idMoneda = _currentItem.Articulo.idMoneda,
                    Articulo = new Articulo(){codigo = txtArticuloCodigo.Text},
                    cantidadEnviada = txtArticuloCantidadEnviados.Text.ToDecimalOrDefault(),
                    costoUnitario = txtArticuloCosto.Text.ToDecimalOrDefault(),
                    idArticulo = _currentItem.idArticulo,
                    PedimentoPorDetalleDeTraspasoes = _currentItem.PedimentoPorDetalleDeTraspasoes
                };
            }
        }

        public DetallesDeTraspaso SelectedItem
        {
            get { return dgDetalle.SelectedIndex >= 0 ? (DetallesDeTraspaso) dgDetalle.SelectedItem : null; }
        }

        public void Show(VMTraspaso transfer)
        {
            //EmpresaAsociada = Destino, EmpresaAsociada1 = Origen
            txtEmpresaAsociadaOrigen.Text = transfer.EmpresasAsociada1.nombre;
            _sourceCompany = transfer.EmpresasAsociada1;

            _isLocal = transfer.EmpresasAsociada1.idEmpresaLocal.HasValue;

            txtEmpresaAsociadaDestino.Text = transfer.EmpresasAsociada.nombre;
            _destinationCompany = transfer.EmpresasAsociada;

            if (IsBeingProcessed)
            {
                //Se debe mostrar el folio remoto que esta guardado en la solicitud de traspaso
                if (transfer.TransferRequest.isValid() && transfer.TransferRequest.idTraspaso.isValid())
                {
                    lblFolioOrigen.Content = transfer.TransferRequest.folio.ToString();
                }
            }
            else
            {
                if (_isLocal)
                {
                    //Se debe mostrar el folio remoto guardado en la transferencia
                    lblFolioDestino.Content = transfer.folioRemoto;
                }
                else
                {
                    lblFolioOrigen.Content = transfer.folioRemoto;
                }
            }

            txtFolio.Text = transfer.folio.ToString();
            txtDescripcion.Text = transfer.descripcion;
            txtTipoDeCambio.Text = transfer.tipoDeCambio.ToDecimalString();
            dpFecha.SelectedDate = transfer.fechaHora;

            _transfer = transfer;
            _transferRequest = transfer.TransferRequest;
            _idTraspaso = transfer.idTraspaso;


            dgDetalle.ItemsSource = null;
            dgDetalle.ItemsSource = transfer.Detalle;

            SetEnvironment((StatusDeTraspaso)transfer.idEstatusDeTraspaso);
        }

        public void Show(DetallesDeTraspaso detail)
        {
            txtArticuloCodigo.Text = detail.Articulo.codigo;
            txtArticuloDescripcion.Text = detail.Articulo.descripcion;
            txtArticuloCantidadEnviados.Text = detail.cantidadEnviada.ToDecimalString();
            txtArticuloCantidadAceptados.Text = detail.cantidadAceptada.HasValue ? detail.cantidadAceptada.Value.ToDecimalString() : "";
            txtArticuloUnidad.Text = detail.Articulo.UnidadesDeMedida.descripcion;
            txtArticuloCosto.Text = detail.costoUnitario.ToDecimalString();
            _currentItem = detail;

            //Si se esta aceptando un traspaso, se hace focus al textbox de cantidad aceptada
            if (IsBeingProcessed)
            {
                txtArticuloCantidadAceptados.Focus();
            }
            else
            {
                txtArticuloCantidadEnviados.Focus();
            }
        }

        public void ShowStock(decimal stock)
        {
            lblExistencia.Content = stock.ToDecimalString();
        }

        public void ClearItem()
        {
            txtArticuloCosto.Clear();
            txtArticuloUnidad.Clear();
            txtArticuloCantidadEnviados.Clear();
            txtArticuloDescripcion.Clear();
            txtArticuloCantidadAceptados.Clear();
            txtArticuloCodigo.Clear();
            lblExistencia.Content = "";
            dpFecha.SelectedDate = DateTime.Today;

            _currentItem = new DetallesDeTraspaso(){Articulo = new Articulo()};
        }

        private void SetEnvironment(StatusDeTraspaso status)
        {
            txtArticuloCantidadEnviados.IsEnabled = status.Equals(StatusDeTraspaso.Nuevo) || status.Equals(StatusDeTraspaso.Registrado);
            txtArticuloCantidadAceptados.IsReadOnly = true;
            dgDetalle.IsReadOnly = status.Equals(StatusDeTraspaso.Total) || status.Equals(StatusDeTraspaso.Parcial) || status.Equals(StatusDeTraspaso.Rechazado);
            lblRechazado.Visibility = System.Windows.Visibility.Hidden;
            btnRechazar.IsEnabled = false;
            btnListarArticulos.IsEnabled = true;
            txtArticuloCodigo.IsReadOnly = false;
            lblTituloFolioOrigen.Visibility = Visibility.Hidden;
            lblTituloFolioDestino.Visibility = Visibility.Hidden;
            lblFolioDestino.Visibility = Visibility.Hidden;
            lblFolioOrigen.Visibility = Visibility.Hidden;

            switch (status)
            {
                case StatusDeTraspaso.Nuevo:
                    btnRegistrar.IsEnabled = true;
                    btnRechazar.IsEnabled = false;
                    txtArticuloCantidadEnviados.IsReadOnly = false;
                    txtArticuloCantidadAceptados.IsReadOnly = true;
                    break;
                case StatusDeTraspaso.Registrado:
                    //Aqui se deberia manejar dependiendo de si es la empresa que emite o recibe
                    if (IsBeingProcessed)
                    {
                        //Se esta procesando el traspaso
                        btnRegistrar.IsEnabled = true;
                        btnRechazar.IsEnabled = true;
                        btnListarArticulos.IsEnabled = false;
                        txtArticuloCodigo.IsReadOnly = true;
                        txtArticuloCantidadEnviados.IsReadOnly = true;
                        txtArticuloCantidadAceptados.IsReadOnly = false;
                        lblTituloFolioOrigen.Visibility = Visibility.Visible;
                        lblFolioOrigen.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        //Se esta editando en la empresa que emite
                        btnRegistrar.IsEnabled = true;
                        btnRechazar.IsEnabled = false;
                        txtArticuloCantidadEnviados.IsReadOnly = false;
                        txtArticuloCantidadAceptados.IsReadOnly = true;
                    }
                    break;
                case StatusDeTraspaso.Parcial:
                    btnRegistrar.IsEnabled = false;
                    btnRechazar.IsEnabled = false;
                    btnListarArticulos.IsEnabled = false;
                    txtArticuloCantidadEnviados.IsReadOnly = false;
                    txtArticuloCantidadAceptados.IsReadOnly = false;
                    txtArticuloCodigo.IsReadOnly = true;
                    if (_isLocal)
                    {
                        lblTituloFolioDestino.Visibility = Visibility.Visible;
                        lblFolioDestino.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        lblTituloFolioOrigen.Visibility = Visibility.Visible;
                        lblFolioOrigen.Visibility = Visibility.Visible;
                    }
                    break;
                case StatusDeTraspaso.Total:
                    btnRegistrar.IsEnabled = false;
                    btnRechazar.IsEnabled = false;
                    btnListarArticulos.IsEnabled = false;
                    txtArticuloCantidadEnviados.IsReadOnly = false;
                    txtArticuloCantidadAceptados.IsReadOnly = false;
                    txtArticuloCodigo.IsReadOnly = true;
                    if (_isLocal)
                    {
                        lblTituloFolioDestino.Visibility = Visibility.Visible;
                        lblFolioDestino.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        lblTituloFolioOrigen.Visibility = Visibility.Visible;
                        lblFolioOrigen.Visibility = Visibility.Visible;
                    }
                    break;
                case StatusDeTraspaso.Rechazado:
                    btnRegistrar.IsEnabled = false;
                    btnRechazar.IsEnabled = false;
                    btnListarArticulos.IsEnabled = false;
                    txtArticuloCantidadEnviados.IsReadOnly = false;
                    txtArticuloCantidadAceptados.IsReadOnly = false;
                    txtArticuloCodigo.IsReadOnly = true;
                    lblRechazado.Visibility = System.Windows.Visibility.Visible;
                    break;
                default:
                    break;
            }
        }
    }
}
