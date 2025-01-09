namespace SettingsService.Exceptions;

public class SettingsLoadException :Exception
{
    public SettingsLoadException()
        : base("Error loading settings.")
    { }

    public SettingsLoadException(string message)
        : base(message)
    { }

    public SettingsLoadException(string message, Exception inner)
        : base(message, inner)
    { }
}