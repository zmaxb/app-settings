using SettingsService;
using static System.Console;

namespace AppSettingsProto;

public class AppSettings : SettingsBase
{
    public new static readonly int ActualSettingsVersion = 2;
    public bool ShowWelcomeMessage { get; set; } = true;
    public string SuperField { get; set; } = string.Empty;
}

internal class PersonalSettings : SettingsBase
{
    public string UserName { get; set; } = string.Empty;
    public bool AllowInput { get; set; } = true;

}

internal class GlobalSettings : SettingsBase
{
    public int SomeCounter { get; set; }
    public bool AllowMessages { get; set; }
}

internal static class Program
{

    private static AppSettings _appSettings = new AppSettings();
    private static PersonalSettings _personalSettings = new PersonalSettings();
    
    static async Task Main(string[] args)
    {
        await InitAllSettingsAsync();
        // WriteLine(_appSettings.ShowWelcomeMessage);
        WriteLine(_personalSettings.UserName);
    }

    private static async Task InitAllSettingsAsync()
    {
    
        var userProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        AppSettingsMigrator appMigrator = new(AppSettings.ActualSettingsVersion); 
        
        //  AppSettings
        var appSettingsManager = new JsonFileSettingsManager<AppSettings, AppSettingsMigrator>(
            Path.Combine(userProfilePath, "AppSettings.config"), appMigrator);
        
        _appSettings = await appSettingsManager.LoadSettingsAsync();
        
        // PersonalSettings
        // var personalSettingsManager = new JsonFileSettingsManager<PersonalSettings>(
        //     Path.Combine(userProfilePath, "PersonalSettings.config"), TODO);
        // _personalSettings = await personalSettingsManager.LoadSettingsAsync();
         // _personalSettings.UserName = "Max";
         // await personalSettingsManager.SaveSettingsAsync(_personalSettings);

    }
}
