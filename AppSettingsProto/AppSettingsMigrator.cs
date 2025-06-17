using SettingsService;

namespace AppSettingsProto;

public class AppSettingsMigrator(int baseActualSettingsVersion)
    : SettingsMigratorBase<AppSettings>(baseActualSettingsVersion)
{
    public override bool Migrate(ref AppSettings settings, ref dynamic? settingsAsDynamic)
    {
        if (settings.Version != ActualSettingsVersion)
        {
            string unused = settingsAsDynamic?.ShowWelcomeMessage;
            return true;
        }
        // ReSharper disable once RedundantIfElseBlock
        else
        {
            return false;
        }
    }
}