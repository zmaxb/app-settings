namespace SettingsService.Exceptions;

public class SettingsMigrateException : Exception
{
    public SettingsMigrateException()
        : base("Error migrate settings.")
    {
    }

    public SettingsMigrateException(string message)
        : base(message)
    {
    }

    public SettingsMigrateException(string message, Exception inner)
        : base(message, inner)
    {
    }
}