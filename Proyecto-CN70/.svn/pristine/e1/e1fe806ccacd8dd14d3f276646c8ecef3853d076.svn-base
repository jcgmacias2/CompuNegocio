using Aprovi.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Presenters
{
    public class ItemCommentPresenter
    {
        private IItemCommentView _view;

        public ItemCommentPresenter(IItemCommentView view)
        {
            _view = view;

            _view.Save += Save;
        }

        private void Save()
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
