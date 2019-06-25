using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TeamX.Security.AuthServer;

namespace WebApplication21
{
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


            // make identityserver4 available to pipeline objects
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()     // uses a fake dev only certificate somehow
                .AddInMemoryApiResources(Config.GetApiResources())    // set the allowed resources that we can authenticate for, our token will only work with these apis'/resources
                .AddInMemoryClients(Config.GetClients()); // clients that are known and registered for authenticating.  uses client id and secret as sort of user/password.
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            // puts the identityserver bits into the pipeline, must be before usemvc().
            // since this is just an auth server, all it will do is lookup clients and their api resources and then return a jwt token with that info
            app.UseIdentityServer();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
