using Aprovi.Helpers;
using Microsoft.Win32;
using System;
using System.Windows;

namespace Aprovi.Views
{
    public class BaseView : Window, IBaseView
    {

        public void ShowWindow()
        {
            this.ShowDialog();
        }

        public void ShowWindowIndependent()
        {
            this.Show();
        }

        public void CloseWindow()
        {
            this.Close();
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message, "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ShowMessage(string textFormat, params object[] arguments)
        {
            MessageBox.Show(string.Format(textFormat, arguments), "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ShowError(Exception ex)
        {
            Logger.Log(ex);
            MessageBox.Show(ex.Message, "Advertencia", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        public void ShowError(string message)
        {
            MessageBox.Show(message, "Advertencia", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        public MessageBoxResult ShowMessageWithOptions(string message)
        {
            return MessageBox.Show(message, "Confirmación", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
        }

        public string OpenFileFinder(string fileFilter)
        {
            OpenFileDialog openFileDialog;

            openFileDialog = new OpenFileDialog();
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.Filter = fileFilter;
            openFileDialog.Multiselect = false;
            openFileDialog.ShowDialog();

            return openFileDialog.FileName;
        }

        public string OpenFolderFinder(string windowCaption)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;

            folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            folderBrowserDialog.Description = windowCaption;
            folderBrowserDialog.ShowNewFolderButton = true;
            folderBrowserDialog.ShowDialog();

            return folderBrowserDialog.SelectedPath;
        }

    }
}
