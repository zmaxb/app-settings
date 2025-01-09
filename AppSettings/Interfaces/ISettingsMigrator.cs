namespace SettingsService.Interfaces;

public interface ISettingsMigrator<T>
{
    bool Migrate(ref T settings, ref dynamic settingsAsDynamic);
}