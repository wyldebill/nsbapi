using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NServiceBus;

namespace WebApplication1
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

            services.AddMvcCore()
                .AddAuthorization()  // this api can be protected, sets up the di needed for the pipeline below
                .AddJsonFormatters();  // this api can return json documents


            // we use identityserver4.accesstokenvalidation nuget only for this api.  it looks for bearer tokens, but can't produce them.
            services.AddAuthentication("Bearer")   // by default, look for bearer tokens in headers for authentication
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "https://localhost:44301";   // where to go when jwt token is needed
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "api1";                       // this identifies our api to the auth server.
                    
                });

            var endpointConfiguration = new EndpointConfiguration("APIEvents.Sender");
            var transport = endpointConfiguration.UseTransport<LearningTransport>();
            endpointConfiguration.SendOnly();

            var routing = transport.Routing();
            routing.RouteToEndpoint(
                assembly: typeof(ApiCalledEvent).Assembly,
                destination: "APIEvents.Endpoint");
            var endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

            services.AddSingleton<IMessageSession>(endpoint);



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

            app.UseAuthentication();  // put authentication into the pipeline based on the setup above.  
            app.UseHttpsRedirection();
            app.UseMvc();


        }
    }
}
