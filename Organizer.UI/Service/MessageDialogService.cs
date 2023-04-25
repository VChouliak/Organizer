using System.Windows;

namespace Organizer.UI.Service
{
    public class MessageDialogService : IMessageDialogService
    {
        public void ShowInfoDialog(string text)
        {
            MessageBox.Show(text, "Info");
        }

        public MessageDialogResult ShowOkCancelDialog(string text, string title)
        {
            var result = MessageBox.Show(text, title, MessageBoxButton.OKCancel);
            return result == MessageBoxResult.OK ? MessageDialogResult.OK : MessageDialogResult.Cancel;
        }
    }



}
