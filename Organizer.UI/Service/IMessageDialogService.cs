namespace Organizer.UI.Service
{
    public interface IMessageDialogService
    {
        MessageDialogResult ShowOkCancelDialog(string text, string title);
    }


    public enum MessageDialogResult
    {
        OK,
        Cancel
    }
}