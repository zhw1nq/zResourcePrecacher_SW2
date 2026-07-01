namespace zResourcePrecacher;

using System.Text.Json.Serialization;

public sealed class PluginConfig
{
    [JsonPropertyName("Resources")]
    public HashSet<string> resourceList { get; set; } = new();

    [JsonPropertyName("Log")]
    public bool log { get; set; } = true;

    [JsonPropertyName("Version")]
    public int version { get; set; } = 5;
}
