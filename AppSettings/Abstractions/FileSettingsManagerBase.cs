using SettingsService.Interfaces;

namespace SettingsService;

public abstract class FileSettingsManagerBase<TSettings> : ISettingsManager<TSettings>
    where TSettings : SettingsBase
{
    protected FileSettingsManagerBase(string settingsFilePath)
    {
        SettingsFilePath = settingsFilePath;
    }

    public string SettingsFilePath { get; protected set; }

    public abstract Task<TSettings> LoadSettingsAsync();

    public abstract Task SaveSettingsAsync(TSettings settings);

    public abstract Task<TSettings> GetDefaultSettingsAsync();
}