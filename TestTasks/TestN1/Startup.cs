using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TestN1.Infrastructure;
using TestTasks.Data.Domains;

namespace TestN1;

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
        services.Configure<CookiePolicyOptions>(options =>
        {
            options.CheckConsentNeeded = _ => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });

        services.AddSession(options =>
        {
            // Set a short timeout for easy testing.
            options.IdleTimeout = TimeSpan.FromSeconds(120);
            options.Cookie.HttpOnly = true;
        });

        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });
        services.AddMvc(options => { options.Filters.Add(typeof(HttpServiceExceptionFilter)); });

        services.AddTransient<EntityDataProvider>();
        services.AddOptions();

        services.AddSingleton(Configuration);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        loggerFactory.CreateLogger("Info");

        if (env.EnvironmentName.Equals("Development")) app.UseDeveloperExceptionPage();

        app.UseCookiePolicy();

        app.UseSession();

        //app.UseMvcWithDefaultRoute();
        app.UseEndpoints(a => a.MapDefaultControllerRoute());
        //app.UseMvc();
    }
}