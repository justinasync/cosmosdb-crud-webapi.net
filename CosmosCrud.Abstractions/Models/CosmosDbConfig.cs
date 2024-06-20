namespace DotnetAssessment.Abstractions.Models;

public class CosmosDbConfig
{
    public string EndpointUri { get; set; } = string.Empty;
    public string EndpointPrimaryKey { get; set; } = string.Empty;
    public string DatabaseId { get; set; } = string.Empty;
    public string ApplicationName { get; set; } = string.Empty;
}
