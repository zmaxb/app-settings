using SettingsService;
using static System.Console;

// ReSharper disable UnusedMember.Global

namespace AppSettingsProto;

public class AppSettings : SettingsBase
{
    public static readonly int ActualSettingsVersion = 3;
    public bool ShowWelcomeMessage { get; set; } = true;
    public string SuperField { get; set; } = "Max";
}

internal static class Program
{
    private static AppSettings _appSettings = new();
    private static JsonFileSettingsManager<AppSettings, AppSettingsMigrator> _appSettingsManager = null!;

    private static async Task Main()
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