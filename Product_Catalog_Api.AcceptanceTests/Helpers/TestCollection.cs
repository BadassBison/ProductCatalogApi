using Xunit;

namespace Product_Catalog_Api.AcceptanceTests.Helpers
{
  public class TestCollection
  {
    [CollectionDefinition(TestBase.TestCollectionName)]
    public class CollectionFixture :
      ICollectionFixture<TestFixture>
    {
    }
  }
}
