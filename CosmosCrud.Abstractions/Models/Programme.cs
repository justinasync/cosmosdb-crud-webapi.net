namespace DotnetAssessment.Abstractions.Models;

public class Programme : IItem
{
    public string Id { get; set; } = string.Empty;

    public string PartitionKey => this.Id;

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public List<Question> Questions { get; set; } = new();
}
