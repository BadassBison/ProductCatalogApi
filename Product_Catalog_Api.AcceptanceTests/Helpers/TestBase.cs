namespace Product_Catalog_Api.AcceptanceTests.Helpers
{
  public class TestBase
  {
    public const string TestCollectionName = "Product_Catalog_ApiSetupCollection";

    protected TestServer Server;
    protected HttpClient Client { get; }
    private IWebHostBuilder HostBuilder { get; }

    // FIXME: Add dbcontext
    // protected÷ static Product_Catalog_ApiContext DbContext;

    public TestBase(TestFixture testFixture)
    {
      // FIXME: Add dbcontext
      // DbContext = testFixture.DbContext;

      // CleanOutTestData();

      // if (Server != null) return;

      // HostBuilder = new WebHostBuilder()
      //   .ConfigureAppConfiguration((builderContext, config) =>
      //   {
      //     config.AddJsonFile("appsettings.json", false, true);
      //   })
      //   .UseEnvironment("Development")
      //   .UseStartup<TestStartup>();
      // SetupTestingServer();

      // Client = Server.CreateClient();
    }

    private static void CleanOutTestData()
    {
      // DbContext.Products.RemoveRange(DbContext.Products);
      // DbContext.SaveChanges();
    }

    private void SetupTestingServer()
    {
      // HostBuilder.ConfigureServices(services => { services.AddSingleton(DbContext); });
      // Server = new TestServer(HostBuilder);
    }
  }
}
