using System.Text.Json.Serialization;

public class SMonitorModel
{
    [JsonPropertyName("VCP Code")]
    public string? VCPCode { get; set; }
    [JsonPropertyName("VCP Code Name")]
    public string? VCPCodeName { get; set; }
    [JsonPropertyName("Read-Write")]
    public string? ReadWrite { get; set; }
    [JsonPropertyName("Current Value")]
    public string? CurrentValue { get; set; }
    [JsonPropertyName("Maximum Value")]
    public string? MaximumValue { get; set; }
    [JsonPropertyName("Possible Values")]
    public string? PossibleValues { get; set; }
}