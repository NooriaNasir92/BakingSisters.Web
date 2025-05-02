using MudBlazor;

namespace BakingSisters.Web.Services;

public interface IToastService
{
    void ShowSuccess(string message, string title = "");
    void ShowError(string message, string title = "");
    void ShowWarning(string message, string title = "");
    void ShowInfo(string message, string title = "");
}

public class ToastService : IToastService
{
    private readonly ISnackbar _snackbar;

    public ToastService(ISnackbar snackbar)
    {
        _snackbar = snackbar;
        // Configure default settings
        _snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
        _snackbar.Configuration.ShowCloseIcon = true;
        _snackbar.Configuration.VisibleStateDuration = 5000; // 5 seconds
        _snackbar.Configuration.HideTransitionDuration = 500;
        _snackbar.Configuration.ShowTransitionDuration = 500;
    }

    public void ShowSuccess(string message, string title = "")
    {
        Show(message, title, Severity.Success);
    }

    public void ShowError(string message, string title = "")
    {
        Show(message, title, Severity.Error);
    }

    public void ShowWarning(string message, string title = "")
    {
        Show(message, title, Severity.Warning);
    }

    public void ShowInfo(string message, string title = "")
    {
        Show(message, title, Severity.Info);
    }

    private void Show(string message, string title, Severity severity)
    {
        var finalMessage = string.IsNullOrEmpty(title) ? message : $"{title}: {message}";
        _snackbar.Add(finalMessage, severity);
    }
} 