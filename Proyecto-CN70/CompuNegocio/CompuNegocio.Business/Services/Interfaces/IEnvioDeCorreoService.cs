using Aprovi.Data.Models;
using System.Collections.Generic;

namespace Aprovi.Business.Services
{
    public interface IEnvioDeCorreoService
    {
        void SendTestMail(CuentasGuardian account);

        void SendMail(Factura invoice);

        void SendMail(AbonosDeFactura fiscalPayment);

        void SendMail(Pago payment);

        void SendMail(List<ComprobantesEnviado> pendingReceipts);

        void SendMail(NotasDeCredito creditNote);

        void SendMail(Cotizacione quote, Opciones_Envio_Correo option, string givenEmail);
    }
}
