namespace Organizer.UI.Service
{
    public interface IMessageDialogService
    {
        void ShowInfoDialog(string text);
        MessageDialogResult ShowOkCancelDialog(string text, string title);
    }


    public enum MessageDialogResult
    {
        OK,
        Cancel
    }
}