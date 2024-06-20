# WebAPI with CosmosDB

Demonstrates CRUD with Alba and FluentAssertions for integration testing.

## Requirements

- .NET 8 SDK
- CosmosDB connection

## Running the API

- Add a `appsettings.Development.json` file to `CosmosCrud.Api` with the following content:

  ```json
  {
    "Cosmos": {
      "EndpointUri": "YOUR_COSMOS_ENDPOINT",
      "EndpointKey": "YOUR_COSMOS_KEY",
      "DatabaseId": "AnyDatabaseIdWorksForCosmosEmulator",
      "ApplicationName": "AnyNameWorks"
      }
  }
  ```
- Run the `https` profile of `CosmosCrud.Api` and head on to https://localhost:7026/swagger/index.html

## Running the tests

Since the tests are integration tests, they require a CosmosDB connection.

- Add a `appsettings.Testing.json` file to `CosmosCrud.Api` with the following content:

  ```json
  {
    "Cosmos": {
      "EndpointUri": "YOUR_COSMOS_ENDPOINT",
      "EndpointKey": "YOUR_COSMOS_KEY",
      "ApplicationName": "AnyNameWorks"
      }
  }
  ```
  Note that the `DatabaseId` is omitted in test config because the tests will create a new database for each test run.

- Run `dotnet test` in the root directory of the solution.

## Improvements

Some features are neglected due to limited time. Some I can recall:

1. Fat controllers: I could've decoupled the features layer from the API layer with the mediator pattern. Not a functional limitation because it can easily be refactored into services at the very least.
2. Exposed domain models: There should be more view models and explicit mappings from `IItem`, rather than expose the view models directly. Also not a functional limitation because there are no sensitive properties here.
3. Should expose a locally declared `QueryDefinition` instead of `Microsoft.Azure.Cosmos.QueryDefinition`. This removes the dependency on `Microsoft.Azure.Cosmos` nuget for the Abstractions project.
4. Broader test coverage. Neglected because the added scenario demonstrates how it should be done.
