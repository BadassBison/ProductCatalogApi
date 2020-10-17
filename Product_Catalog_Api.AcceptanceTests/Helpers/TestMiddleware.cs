namespace Product_Catalog_Api.AcceptanceTests.Helpers
{
  public class TestMiddleware
  {
    private readonly RequestDelegate _next;

    public TestMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
      httpContext.User.AddIdentity(new ClaimsIdentity("password"));
      await _next.Invoke(httpContext);
    }
  }
}
