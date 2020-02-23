using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace NswLicensePlateLookupTests
{
    public class TestingWebApplicationFactory<T> : WebApplicationFactory<T> where T: class
    {
        protected override IWebHostBuilder CreateWebHostBuilder() => 
            base.CreateWebHostBuilder()
                .ConfigureAppConfiguration(
                    config => config.AddEnvironmentVariables("ASPNETCORE_ENVIRONMENT"));
    }
}