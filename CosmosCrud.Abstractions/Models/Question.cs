namespace DotnetAssessment.Abstractions.Models;

public class Question : IItem
{
    public string Id { get; set; } = string.Empty;

    public string ProgrammeId { get; set; } = string.Empty;

    public string PartitionKey => this.ProgrammeId;

    public string Text { get; set; } = string.Empty;

    public QuestionKind Kind { get; set; }

    public List<string>? Options { get; set; }
}
