using System.Collections.Generic;
using System.Text.Json;

namespace ItemsAdministration.Common.Infrastructure.Readers.Interfaces;

public interface IDictionaryJsonFileReader
{
    IReadOnlyDictionary<string, JsonElement> Read(string jsonResourceName);
}
