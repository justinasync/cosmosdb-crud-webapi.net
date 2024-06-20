namespace DotnetAssessment.Abstractions.Models;

public class Question
{
    public string Id { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;

    public QuestionKind Kind { get; set; }

    public List<string>? Options { get; set; }
}
