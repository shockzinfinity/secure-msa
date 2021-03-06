using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Test;
using IdentityServerHost.Quickstart.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServer
{
  public class Startup
  {
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      // for login ui
      services.AddControllersWithViews();

      //services.AddIdentityServer()
      //  .AddInMemoryClients(Config.Clients)
      //  .AddInMemoryIdentityResources(Config.IdentityResources)
      //  .AddInMemoryApiResources(Config.ApiResources)
      //  .AddInMemoryApiScopes(Config.ApiScopes)
      //  .AddTestUsers(Config.TestUsers)
      //  .AddDeveloperSigningCredential();
      services.AddIdentityServer()
        .AddInMemoryClients(Config.Clients)
        .AddInMemoryApiScopes(Config.ApiScopes)
        .AddInMemoryIdentityResources(Config.IdentityResources)
        //.AddTestUsers(Config.TestUsers)
        .AddTestUsers(TestUsers.Users)
        .AddDeveloperSigningCredential();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      // for login ui - before routing
      app.UseStaticFiles();

      app.UseRouting();
      app.UseIdentityServer();
      app.UseAuthorization(); // for login ui

      //app.UseEndpoints(endpoints =>
      //{
      //  endpoints.MapGet("/", async context =>
      //        {
      //          await context.Response.WriteAsync("Hello World!");
      //        });
      //});
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapDefaultControllerRoute();
      });
    }
  }
}
