using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NswLicensePlateLookup.Interfaces;
using NswLicensePlateLookup.Services;
using Refit;

namespace NswLicensePlateLookup
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddRefitClient<IServiceNswApi>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://my.service.nsw.gov.au"));

            services.AddSingleton<IServiceNswTransactionTokenHelper, ServiceNswTransactionTokenHelper>();
            services.AddSingleton<IServiceNswRequestHelper, ServiceNswRequestHelper>();
            services.AddSingleton<IPlateLookupService, PlateLookupService>();
            services.AddHostedService<TransactionTokenRefreshBackgroundTask>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
