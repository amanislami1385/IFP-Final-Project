namespace AfghanFutureHope.Services;

public enum ToastKind { Success, Error, Info }
public record ToastMessage(Guid Id, string Text, ToastKind Kind);

public class ToastService
{
    public event Action<ToastMessage>? OnShow;
    public void Show(string text, ToastKind kind = ToastKind.Success) =>
        OnShow?.Invoke(new ToastMessage(Guid.NewGuid(), text, kind));
}
