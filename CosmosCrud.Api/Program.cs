using DotnetAssessment.Abstractions.Models;
using DotnetAssessment.Abstractions.Repositories;
using DotnetAssessment.Features.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.Configure<CosmosDbConfig>(builder.Configuration.GetSection("CosmosDb"));
builder.Services.AddSingleton<IRepository, CosmosDbRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
