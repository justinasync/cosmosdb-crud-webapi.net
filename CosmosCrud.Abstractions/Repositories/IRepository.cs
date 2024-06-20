using DotnetAssessment.Abstractions.Models;
using Microsoft.Azure.Cosmos;

namespace DotnetAssessment.Abstractions.Repositories;

public interface IRepository
{
    IAsyncEnumerable<TItem> GetItems<TItem>(QueryDefinition? query = null,
        CancellationToken ct = default)
        where TItem : class, IItem;

    Task<TItem?> GetItem<TItem>(string id, string partitionKey, CancellationToken ct = default)
        where TItem : class, IItem;

    Task<TItem?> GetItem<TItem>(QueryDefinition query, CancellationToken ct = default)
        where TItem : class, IItem;

    Task AddItem<TItem>(TItem item, CancellationToken ct = default)
        where TItem : class, IItem;

    Task UpdateItem<TItem>(string id, string partitionKey, TItem item, CancellationToken ct = default)
        where TItem : class, IItem;

    Task DeleteItem<TItem>(string id, string partitionKey, CancellationToken ct = default)
        where TItem : class, IItem;
}
