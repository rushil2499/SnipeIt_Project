namespace SnipeIt.Tests.Models;

public record AssetModel
{
    public string AssetTag { get; init; } = string.Empty;
    public string ModelName { get; init; } = "Macbook Pro 13\"";
    public string Status { get; init; } = "Ready to Deploy";
    public string AssignedUser { get; init; } = string.Empty;
}