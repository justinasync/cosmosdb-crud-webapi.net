namespace DotnetAssessment.Abstractions.ViewModels;

public class CreateAnswerViewModel
{
    public string QuestionId { get; init; } = string.Empty;

    public string? ParagraphResponse { get; init; }

    public bool? YesNoResponse { get; init; }

    public string? DropdownResponse { get; init; }

    public List<string>? MultipleChoiceResponses { get; init; }

    public DateTime? DateResponse { get; init; }

    public double? NumberResponse { get; init; }
}
