namespace DotnetAssessment.Abstractions.Models;

public class CosmosDbConfig
{
    public string EndpointUri { get; set; } = string.Empty;
    public string EndpointKey { get; set; } = string.Empty;
    public string DatabaseId { get; set; } = Guid.NewGuid().ToString();
    public string ApplicationName { get; set; } = string.Empty;
}
