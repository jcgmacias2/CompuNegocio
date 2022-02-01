using Aprovi.Business.Services;
using Aprovi.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Presenters
{
    public class GuardianManualSendPresenter
    {
        private IGuardianManualSendView _view;
        private IEnvioDeCorreoService _mailer;
        private IComprobanteEnviadoService _sentReceipts;

        public GuardianManualSendPresenter(IGuardianManualSendView view, IEnvioDeCorreoService mailer, IComprobanteEnviadoService sentReceipts)
        {
            _view = view;
            _mailer = mailer;
            _sentReceipts = sentReceipts;

            _view.Quit += Quit;
            _view.Send += Send;
            _view.Load += Load;
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

        private void Send()
        {
            try
            {
                var pending = _sentReceipts.List(true);

                if (pending.Count.Equals(0))
                    throw new Exception("No hay comprobantes pendientes de enviar");

                _mailer.SendMail(pending);

                _view.ShowMessage("Envío de correos finalizado");
                _view.Fill(_sentReceipts.List(true));

            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Load()
        {
            try
            {
                //Cargo la lista de comprobantes pendientes de enviar
                var pending = _sentReceipts.List(true);

                _view.Fill(pending);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
