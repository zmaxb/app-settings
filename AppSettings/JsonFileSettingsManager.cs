using Newtonsoft.Json;
using SettingsService.Exceptions;

namespace SettingsService;

public class JsonFileSettingsManager<TSettings, TMigrator>(string settingsFilePath, TMigrator migrator)
    : FileSettingsManagerBase<TSettings>(settingsFilePath)
    where TSettings : SettingsBase, new()
    where TMigrator : SettingsMigratorBase<TSettings>
{
    public override async Task<TSettings> LoadSettingsAsync()
    {
        try
        {
            if (!File.Exists(SettingsFilePath)) return await GetDefaultSettingsAsync();

            var settings = JsonConvert.DeserializeObject<TSettings>(
                await File.ReadAllTextAsync(SettingsFilePath));

            if (settings is null) throw new JsonSerializationException("Settings deserialization returned null.");

            if (settings.Version == migrator.ActualSettingsVersion) return settings;

            var settingsAsDynamic =
                JsonConvert.DeserializeObject<dynamic>(await File.ReadAllTextAsync(SettingsFilePath));

            if (migrator.Migrate(ref settings, ref settingsAsDynamic))
                settings.Version = migrator.ActualSettingsVersion;

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
            throw HandleSaveSettingsException(ex);
        }
    }

    public override Task<TSettings> GetDefaultSettingsAsync()
    {
        return Task.FromResult(new TSettings());
    }

    private static Exception HandleLoadSettingsException(Exception ex)
    {
        return new SettingsLoadException(GetExceptionMessage(ex), ex);
    }

    private static Exception HandleSaveSettingsException(Exception ex)
    {
        return new SettingsSaveException(GetExceptionMessage(ex), ex);
    }

    private static string GetExceptionMessage(Exception ex)
    {
        return ex switch
        {
            JsonSerializationException =>
                "Error serializing the settings object to JSON. Ensure the object is serializable.",
            UnauthorizedAccessException =>
                "Failed to save settings due to insufficient access rights. Ensure you have write permissions for the file.",
            DirectoryNotFoundException =>
                "The directory for saving settings was not found. Verify the path and ensure it exists.",
            IOException =>
                "An I/O error occurred while accessing the settings file. Check the file path and ensure it is accessible.",
            _ => "An unknown error occurred while saving settings. Please try again later."
        };
    }
}