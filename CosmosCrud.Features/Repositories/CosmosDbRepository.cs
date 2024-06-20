using System.Net;
using System.Runtime.CompilerServices;
using DotnetAssessment.Abstractions;
using DotnetAssessment.Abstractions.Models;
using DotnetAssessment.Abstractions.Repositories;
using DotnetAssessment.Abstractions.Utils;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace DotnetAssessment.Features.Repositories;

public class CosmosDbRepository : IRepository
{
    private readonly CosmosClient cosmosClient;
    private readonly string databaseId;
    private Database? database;
    private bool databaseInitialized;

    public CosmosDbRepository(IOptions<CosmosDbConfig> cosmosDbConfig) : this(new CosmosClient(
            cosmosDbConfig.Value.EndpointUri,
            cosmosDbConfig.Value.EndpointKey,
            new CosmosClientOptions
            {
                ApplicationName = cosmosDbConfig.Value.ApplicationName,
                SerializerOptions =
                    new CosmosSerializationOptions
                    {
                        PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                    }
            }),
        cosmosDbConfig.Value.DatabaseId)
    {
    }

    public CosmosDbRepository(CosmosClient cosmosClient, string databaseId)
    {
        this.cosmosClient = cosmosClient;
        this.databaseId = databaseId;
        databaseInitialized = false;
    }

    public async IAsyncEnumerable<TItem> GetItems<TItem>(QueryDefinition? query = null,
        [EnumeratorCancellation] CancellationToken ct = default)
        where TItem : class, IItem
    {
        var container = await getOrCreateContainer<TItem>(ct);
        var iterator = container.GetItemQueryIterator<TItem>(query ?? new QueryDefinition("SELECT * FROM c"));

        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync(ct);

            foreach (var item in response)
            {
                yield return item;
            }
        }
    }

    public async Task<TItem?> GetItem<TItem>(string id, string partitionKey, CancellationToken ct = default)
        where TItem : class, IItem
    {
        var container = await getOrCreateContainer<TItem>(ct);
        try
        {
            ItemResponse<TItem> response =
                await container.ReadItemAsync<TItem>(id, new PartitionKey(partitionKey), cancellationToken: ct);
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<TItem?> GetItem<TItem>(QueryDefinition query, CancellationToken ct = default) where TItem : class, IItem
    {
        var container = await getOrCreateContainer<TItem>(ct);
        var iterator = container.GetItemQueryIterator<TItem>(query);

        if (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync(ct);
            return response.FirstOrDefault();
        }

        return null;
    }

    public async Task AddItem<TItem>(TItem item, CancellationToken ct = default)
        where TItem : class, IItem
    {
        var container = await getOrCreateContainer<TItem>(ct);
        await container.CreateItemAsync(item, new PartitionKey(item.Id), cancellationToken: ct);
    }

    public async Task UpdateItem<TItem>(string id, string partitionKey, TItem item, CancellationToken ct = default)
        where TItem : class, IItem
    {
        var container = await getOrCreateContainer<TItem>(ct);
        await container.UpsertItemAsync(item, new PartitionKey(partitionKey), cancellationToken: ct);
    }

    public async Task DeleteItem<TItem>(string id, string partitionKey, CancellationToken ct = default)
        where TItem : class, IItem
    {
        var container = await getOrCreateContainer<TItem>(ct);
        await container.DeleteItemAsync<TItem>(id, new PartitionKey(partitionKey), cancellationToken: ct);
    }

    private async Task<Database> getOrCreateDatabase(CancellationToken ct)
    {
        if (!databaseInitialized)
        {
            database = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId, cancellationToken: ct);
            databaseInitialized = true;
        }

        return this.database!;
    }

    private async Task<Container> getOrCreateContainer<TItem>(CancellationToken ct)
        where TItem : class, IItem
    {
        var db = await getOrCreateDatabase(ct);

        return await db.CreateContainerIfNotExistsAsync(typeof(TItem).Name,
            $"/{nameof(IItem.PartitionKey).ToCamelCase()}",
            cancellationToken: ct);
    }
}
