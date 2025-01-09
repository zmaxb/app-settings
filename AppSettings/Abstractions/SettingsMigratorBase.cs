using SettingsService.Interfaces;

namespace SettingsService;

public abstract class SettingsMigratorBase<TSettingsBase>(int actualSettingsVersion) : ISettingsMigrator<TSettingsBase>
{
    public readonly int ActualSettingsVersion = actualSettingsVersion;

    public abstract bool Migrate(ref TSettingsBase settings, ref dynamic settingsAsDynamic);
}