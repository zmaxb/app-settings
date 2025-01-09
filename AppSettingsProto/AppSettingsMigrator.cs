using SettingsService;

namespace AppSettingsProto;

public class AppSettingsMigrator(int baseActualSettingsVersion) : SettingsMigratorBase<AppSettings>(baseActualSettingsVersion)
{
    public override bool Migrate(ref AppSettings settings, ref dynamic settingsAsDynamic)
    {
        if (settings.Version != baseActualSettingsVersion)
        {
            string tmp = settingsAsDynamic.ShowWelcomeMessage;
            return true;
        }
        else
        {
            return false;
        }
        
    }
}