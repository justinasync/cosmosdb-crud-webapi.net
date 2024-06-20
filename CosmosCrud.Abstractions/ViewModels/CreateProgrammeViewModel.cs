namespace DotnetAssessment.Abstractions.ViewModels;

public class CreateProgrammeViewModel
{
    public string Title { get; init; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public List<CreateQuestionViewModel> Questions { get; init; } = null!;
}
