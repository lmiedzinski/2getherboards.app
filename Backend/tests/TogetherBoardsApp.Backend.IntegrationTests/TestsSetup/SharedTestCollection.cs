using Xunit;

namespace TogetherBoardsApp.Backend.IntegrationTests.TestsSetup;

[CollectionDefinition("SharedTestCollection")]
public class SharedTestCollection : ICollectionFixture<TogetherBoardsAppFactory>
{
}