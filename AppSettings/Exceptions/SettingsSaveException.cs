namespace SettingsService.Exceptions;

public class SettingsSaveException : Exception
{
    public SettingsSaveException()
        : base("Error saving settings.")
    { }

    public SettingsSaveException(string message)
        : base(message)
    { }

    public SettingsSaveException(string message, Exception inner)
        : base(message, inner)
    { }
}