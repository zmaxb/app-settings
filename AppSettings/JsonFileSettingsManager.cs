using Newtonsoft.Json;
using SettingsService.Exceptions;
using SettingsService.Interfaces;

namespace SettingsService;

public class JsonFileSettingsManager<TSettings, TMigrator>(string settingsFilePath, TMigrator migrator)
    : FileSettingsManagerBase<TSettings>(settingsFilePath)
    where TSettings : SettingsBase, new()
    where TMigrator : SettingsMigratorBase<TSettings>
{
    private readonly TMigrator _migrator = migrator;

    public override async Task<TSettings> LoadSettingsAsync()
    {
        try
        {
            var settings = JsonConvert.DeserializeObject<TSettings>(
                await File.ReadAllTextAsync(SettingsFilePath));

            if (settings is null)
            {
                throw new JsonSerializationException("Settings deserialization returned null.");
            }

            if (settings.Version != _migrator.ActualSettingsVersion) ;
            {
                var settingsAsDynamic = JsonConvert.DeserializeObject<dynamic>(await File.ReadAllTextAsync(SettingsFilePath));
                _migrator.Migrate(ref settings, ref settingsAsDynamic);
            }

            return settings;
        }
        catch (Exception ex)
        {
            throw HandleLoadSettingsException(ex);
        }
    }

    public override async Task SaveSettingsAsync(TSettings settings)
    {
        try
        {
            var jsonString = JsonConvert.SerializeObject(settings, Formatting.Indented);
            await File.WriteAllTextAsync(SettingsFilePath, jsonString);
        }
        catch (Exception ex)
        {
            HandleSaveSettingsException(ex);
        }
    }

    public override Task<TSettings> GetDefaultSettingsAsync()
    {
        return Task.FromResult(new TSettings());
    }

    private static Exception HandleLoadSettingsException(Exception ex)
    {
        return ex switch
        {
            JsonSerializationException jsonSerializationException => new SettingsLoadException(
                "Error serializing the settings object to JSON. Ensure the object is serializable.", ex),

            UnauthorizedAccessException jsonSerializationException => new SettingsLoadException(
                "Failed to load settings due to insufficient access rights." +
                "Ensure you have read permissions for the file.", ex),

            DirectoryNotFoundException jsonSerializationException => new SettingsLoadException(
                "Failed to load settings due to insufficient access rights." +
                "Ensure you have read permissions for the file.", ex),

            IOException jsonSerializationException => new SettingsLoadException(
                "An I/O error occurred while accessing the settings file." +
                "Check the file path and ensure it is accessible.", ex),

            _ => new SettingsLoadException(
                "An unknown error occurred while loading settings. Please try again later.", ex)
        };
    }

    private static void HandleSaveSettingsException(Exception ex)
    {
        throw ex switch
        {
            JsonSerializationException => new SettingsSaveException(
                "Error serializing the settings object to JSON." + "Ensure the object is serializable.", ex),

            UnauthorizedAccessException => new SettingsSaveException(
                "Failed to save settings due to insufficient access rights." +
                "Ensure you have write permissions for the file.", ex),

            DirectoryNotFoundException => new SettingsSaveException(
                "The directory for saving settings was not found." + "Verify the path and ensure it exists.", ex),

            IOException => new SettingsSaveException(
                "An I/O error occurred while accessing the settings file." +
                "Check the file path and ensure it is accessible.", ex),

            _ => new SettingsSaveException(
                "An unknown error occurred while saving settings." + "Please try again later.", ex)
        };
    }
}