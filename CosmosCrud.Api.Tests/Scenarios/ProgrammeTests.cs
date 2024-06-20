using Alba;
using DotnetAssessment.Abstractions.Models;
using DotnetAssessment.Abstractions.Repositories;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace CosmosCrud.Api.Tests.Scenarios;

public class ProgrammeTests
{
    [Fact]
    public async Task GetProgrammesShouldReturnProgrammesInDatabase()
    {
        await using var host = await AlbaHost.For<Program>(Reusables.HostConfiguration);
        var repository = host.Services.GetRequiredService<IRepository>();

        // Seed the database with a programme
        var programme = new Programme
        {
            Id = Guid.NewGuid().ToString(),
            Title = "Test Programme",
            Description = "Test Description",
            Questions =
            [
                new Question
                {
                    Id = Guid.NewGuid().ToString(),
                    Text = "Test Question",
                    Kind = QuestionKind.MultipleChoice,
                    Options = new List<string> { "Option 1", "Option 2" }
                }
            ]
        };
        await repository.AddItem(programme);

        var result = await host.Scenario(scenario =>
        {
            scenario.Get.Url("/api/programmes");
            scenario.StatusCodeShouldBeOk();
        });
        var data = result.ReadAsJson<List<Programme>>();

        // Assert that the seeded programme was returned
        data.Should().NotBeNull();
        data.Count.Should().Be(1);
        data[0].Id.Should().Be(programme.Id);
        data[0].Questions.Count.Should().Be(1);
        data[0].Questions[0].Id.Should().Be(programme.Questions[0].Id);
    }
}
