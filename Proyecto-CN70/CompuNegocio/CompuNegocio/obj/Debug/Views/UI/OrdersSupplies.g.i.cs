#pragma checksum "..\..\..\..\Views\UI\OrdersSupplies.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "11F053436868BC655B9A06531627F9017838DF69"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Aprovi.Application.Helpers;
using Aprovi.Views;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Aprovi.Views.UI {
    
    
    /// <summary>
    /// OrdersSupply
    /// </summary>
    public partial class OrdersSupply : Aprovi.Views.BaseView, System.Windows.Markup.IComponentConnector {
        
        
        #line 15 "..\..\..\..\Views\UI\OrdersSupplies.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtPedido;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\..\Views\UI\OrdersSupplies.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnListarPedidos;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\..\Views\UI\OrdersSupplies.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker dpFecha;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\..\Views\UI\OrdersSupplies.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtFolioDeSurtido;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\..\Views\UI\OrdersSupplies.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtArticuloCodigo;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\..\Views\UI\OrdersSupplies.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnListarArticulos;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\..\Views\UI\OrdersSupplies.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtArticuloDescripcion;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\..\Views\UI\OrdersSupplies.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtArticuloCantidad;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\..\Views\UI\OrdersSupplies.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtArticuloUnidad;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\..\Views\UI\OrdersSupplies.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtArticuloSurtidas;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\..\Views\UI\OrdersSupplies.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgDetalle;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\..\Views\UI\OrdersSupplies.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCerrar;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\..\Views\UI\OrdersSupplies.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnNuevo;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\..\Views\UI\OrdersSupplies.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnImprimir;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\..\Views\UI\OrdersSupplies.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnRegistrar;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/CompuNegocio;component/views/ui/orderssupplies.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\UI\OrdersSupplies.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.txtPedido = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.btnListarPedidos = ((System.Windows.Controls.Button)(target));
            return;
            case 3:
            this.dpFecha = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 4:
            this.txtFolioDeSurtido = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.txtArticuloCodigo = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.btnListarArticulos = ((System.Windows.Controls.Button)(target));
            return;
            case 7:
            this.txtArticuloDescripcion = ((System.Windows.Controls.TextBox)(target));
            return;
            case 8:
            this.txtArticuloCantidad = ((System.Windows.Controls.TextBox)(target));
            return;
            case 9:
            this.txtArticuloUnidad = ((System.Windows.Controls.TextBox)(target));
            return;
            case 10:
            this.txtArticuloSurtidas = ((System.Windows.Controls.TextBox)(target));
            return;
            case 11:
            this.dgDetalle = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 12:
            this.btnCerrar = ((System.Windows.Controls.Button)(target));
            return;
            case 13:
            this.btnNuevo = ((System.Windows.Controls.Button)(target));
            return;
            case 14:
            this.btnImprimir = ((System.Windows.Controls.Button)(target));
            return;
            case 15:
            this.btnRegistrar = ((System.Windows.Controls.Button)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

