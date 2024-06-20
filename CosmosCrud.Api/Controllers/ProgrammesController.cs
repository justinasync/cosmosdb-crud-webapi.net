using System.Runtime.CompilerServices;
using DotnetAssessment.Abstractions.Models;
using DotnetAssessment.Abstractions.Repositories;
using DotnetAssessment.Abstractions.Utils;
using DotnetAssessment.Abstractions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;

namespace DotnetAssessment.Api.Controllers;

[Route("api/programmes")]
[ApiController]
public class ProgrammesController(IRepository repository) : ControllerBase
{
    private readonly string idColumnName = nameof(IItem.Id).ToCamelCase();
    private readonly string questionsColumnName = nameof(Programme.Questions).ToCamelCase();
    private readonly string questionKindColumnName = nameof(Question.Kind).ToCamelCase();

    [HttpPost]
    public async Task<IActionResult> CreateProgramme([FromBody] CreateProgrammeViewModel body,
        CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var programme = new Programme
        {
            Id = Guid.NewGuid().ToString(),
            Title = body.Title,
            Description = body.Description,
            Questions = body.Questions.Select(q => new Question
            {
                Id = Guid.NewGuid().ToString(),
                Text = q.Text,
                Kind = q.Kind,
                Options = q.Options
            }).ToList()
        };

        await repository.AddItem(programme, ct);

        return CreatedAtAction(nameof(GetProgramme), new { id = programme.Id }, programme);
    }

    [HttpGet]
    public async IAsyncEnumerable<Programme> GetProgrammes([EnumeratorCancellation] CancellationToken ct = default)
    {
        await foreach (var programme in repository.GetItems<Programme>(ct: ct))
        {
            yield return programme;
        }
    }

    [HttpGet("questions/by-kind/{questionKind}")]
    public async Task<ActionResult<List<Question>>>
        GetQuestionsByKind(string questionKind, CancellationToken ct = default)
    {
        var programme = await repository.GetItem<Programme>(
            new QueryDefinition(
                    $"SELECT * FROM c JOIN q IN c.{questionsColumnName} WHERE q.{questionKindColumnName} = @questionKind")
                .WithParameter("@questionKind", questionKind),
            ct);

        if (programme is null)
        {
            return NotFound();
        }

        return Ok(programme.Questions);
    }

    [HttpGet("{programmeId}")]
    public async Task<ActionResult<Programme>> GetProgramme(string programmeId, CancellationToken ct = default)
    {
        var programme = await repository.GetItem<Programme>(programmeId, programmeId, ct);

        if (programme is null)
        {
            return NotFound();
        }

        return Ok(programme);
    }

    [HttpPut("{programmeId}/questions/{questionId}")]
    public async Task<ActionResult> UpdateQuestion(string programmeId, string questionId,
        [FromBody] UpdateQuestionViewModel body)
    {
        var programme = await repository.GetItem<Programme>(
            new QueryDefinition(
                    $"SELECT p, q FROM c p JOIN q IN p.{questionsColumnName} WHERE p.{idColumnName} = @programmeId AND q.{idColumnName} = @questionId")
                .WithParameter("@programmeId", programmeId)
                .WithParameter("@questionId", questionId),
            HttpContext.RequestAborted);

        if (programme is null)
        {
            return NotFound();
        }

        var question = programme.Questions.FirstOrDefault(q => q.Id == questionId);

        if (question is null)
        {
            return NotFound();
        }

        question.Text = body.Text;
        question.Kind = body.Kind;
        question.Options = body.Options;

        await repository.UpdateItem(programmeId, programmeId, programme, HttpContext.RequestAborted);

        return Ok(programme);
    }
}
