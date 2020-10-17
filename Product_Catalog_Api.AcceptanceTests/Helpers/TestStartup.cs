namespace Product_Catalog_Api.AcceptanceTests.Helpers
{
  internal class TestStartup : Startup
  {
    public TestStartup(IConfiguration configuration) : base(configuration) {}

    public override void ConfigureServices(IServiceCollection services)
    {
      base.ConfigureServices(services);
    }

    // FIXME: Add the dbcontext
    // public override void Configure(IApplicationBuilder app, IHostingEnvironment env)
    // {
    //   app.UseMiddleware<TestMiddleware>();
    //   base.Configure(app, env, dbContext);
    // }
  }
}
