using SettingsService;
using static System.Console;

namespace AppSettingsProto;

public class AppSettings : SettingsBase
{
    public static readonly int ActualSettingsVersion = 3;
    public bool ShowWelcomeMessage { get; set; } = true;
    public string SuperField { get; set; } = "Max";
}

internal class PersonalSettings : SettingsBase
{
    public string UserName { get; set; } = "Max";
    public bool AllowInput { get; set; } = true;
}

internal static class Program
{
    private static AppSettings _appSettings = new AppSettings();
    private static JsonFileSettingsManager<AppSettings, AppSettingsMigrator> _appSettingsManager;
    
    private static PersonalSettings _personalSettings = new PersonalSettings();

    static async Task Main(string[] args)
    {
        await InitAllSettingsAsync();
        WriteLine(_appSettings.SuperField);
        
        _appSettings.SuperField = "Updated Value";
        await _appSettingsManager.SaveSettingsAsync(_appSettings);
    }

    private static async Task InitAllSettingsAsync()
    {
        var userProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        AppSettingsMigrator appMigrator = new(AppSettings.ActualSettingsVersion);

        var settingsFilePath = Path.Combine(userProfilePath, "AppSettings.config");
         _appSettingsManager =
            new JsonFileSettingsManager<AppSettings, AppSettingsMigrator>(settingsFilePath, appMigrator);

        _appSettings = await _appSettingsManager.LoadSettingsAsync();
        
    }
}