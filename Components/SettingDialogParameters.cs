namespace Components;

public class SettingDialogParameters<T>(
    IDialogService dialogService,
    string title,
    object contextText,
    object buttonText,
    object buttonColor) : ComponentBase where T : ComponentBase
{
    private string Title { get; set; } = title;
    private object ContentText { get; set; } = contextText;
    private object ButtonText { get; set; } = buttonText;
    private object ButtonColor { get; set; } = buttonColor;
    private readonly DialogParameters _parameters = [];

    private IDialogService DialogService { get; set; } = dialogService;

    private DialogParameters GetDialogParameter()
    {
        _parameters.Add("ContentText", ContentText);
        _parameters.Add("ButtonText", ButtonText);
        _parameters.Add("Color", ButtonColor);

        return _parameters;
    }

    public async Task<DialogResult> SetConfirmDialog()
    {
        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

        var dialog = await DialogService!.ShowAsync<T>(Title, GetDialogParameter(), options);
        return await dialog.Result;
    }
}