namespace DotnetAssessment.Abstractions.Models;

public interface IItem
{
    string Id { get; set; }

    string PartitionKey { get; }
}
