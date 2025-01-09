namespace SettingsService.Interfaces;

public interface ISettingsManager<TSettings> where TSettings : class
{
    Task<TSettings> LoadSettingsAsync();
    Task SaveSettingsAsync(TSettings settings);
    Task<TSettings> GetDefaultSettingsAsync();
}