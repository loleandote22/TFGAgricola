namespace AplicacionTFG.Models;

public record AppConfig
{
    public string? Environment { get; init; }
    public string? ApiUrl { get; init; }
    public List<string>? Icons { get; init; }
}
