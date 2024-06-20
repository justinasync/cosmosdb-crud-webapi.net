namespace DotnetAssessment.Abstractions.Models;

public class Answer : IItem
{
    public string Id { get; set; } = string.Empty;

    public string PartitionKey => this.QuestionId;

    public string QuestionId { get; set; } = string.Empty;

    #region: Additional properties based on question type

    public string? ParagraphResponse { get; set; }

    public bool? YesNoResponse { get; set; }

    public string? DropdownResponse { get; set; }

    public List<string>? MultipleChoiceResponses { get; set; }

    public DateTimeOffset? DateResponse { get; set; }

    public double? NumberResponse { get; set; }

    #endregion
}
