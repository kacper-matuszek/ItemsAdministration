using ItemsAdministration.Common.Infrastructure.Options;
using ItemsAdministration.Common.Infrastructure.Readers.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using JsonReaderOptions = ItemsAdministration.Common.Infrastructure.Options.JsonReaderOptions;

namespace ItemsAdministration.Common.Infrastructure.Readers;

public sealed class DictionaryJsonFileReader : IDictionaryJsonFileReader
{
    private const string JsonFileExtensions = ".json";

    private readonly string _rootPath;
    private readonly ILogger<DictionaryJsonFileReader> _logger;

    public DictionaryJsonFileReader(IOptions<JsonReaderOptions> options, ILogger<DictionaryJsonFileReader> logger)
    {
        _rootPath = options.Value.RootPath;
        _logger = logger;
    }

    public IReadOnlyDictionary<string, JsonElement> Read(string jsonResourceName)
    {
        var filePath = CreateFilePath(jsonResourceName);

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Not found file. Path: {filePath}");

        try
        {
            _logger.LogDebug("Reading json from {FilePath}", filePath);

            using StreamReader file = File.OpenText(filePath);
            var result = JsonSerializer.Deserialize<IReadOnlyDictionary<string, JsonElement>>(file.ReadToEnd());
            return result ?? new Dictionary<string, JsonElement>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Occured error during reading data from json file - {FilePath}", filePath);
            return new Dictionary<string, JsonElement>();
        }
    }

    private string CreateFilePath(string jsonResourceName)
    {
        var filePath = Path.Combine(_rootPath, jsonResourceName);
        if (!filePath.EndsWith(JsonFileExtensions))
            filePath += JsonFileExtensions;

        return filePath;
    }
}
