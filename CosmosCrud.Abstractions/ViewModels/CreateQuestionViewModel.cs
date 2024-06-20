using DotnetAssessment.Abstractions.Models;

namespace DotnetAssessment.Abstractions.ViewModels;

public class CreateQuestionViewModel
{
    public string Text { get; init; } = string.Empty;

    public QuestionKind Kind { get; init; }

    public List<string>? Options { get; init; }

    public string ProgrammeId { get; init; } = string.Empty;
}
