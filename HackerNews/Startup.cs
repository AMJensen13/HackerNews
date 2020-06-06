using HackerNews.Data.Services;
using HackerNews.Domain.Exceptions;
using HackerNews.Domain.Interfaces;
using HackerNews.Domain.Models.CosmosDb;
using HackerNews.Domain.Models.HackerNews;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;

namespace HackerNews
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    });
            
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.Configure<HackerNewsConfig>(Configuration.GetSection("HackerNewsConfig"));
            services.Configure<CosmosDbConfig>(Configuration.GetSection("CosmosDb"));
            services.AddHttpClient<IStoryService, HackerNewsService>();
            services.AddSingleton<ICosmosDbService, CosmosDbService>((provider) =>
            {
                var config = provider.GetRequiredService<IOptions<CosmosDbConfig>>();

                var clientBuilder = new CosmosClientBuilder(config.Value.ConnectionString);
                var client = clientBuilder.WithConnectionModeDirect()
                                       .Build();

                var database = client.GetDatabase(config.Value.DatabaseName);

                if (database == null)
                {
                    throw new CosmosDbException($"Database '{config.Value.DatabaseName}' does not exist.");
                }

                var container = database.GetContainer(config.Value.ContainerName);

                if (container == null)
                {
                    throw new CosmosDbException($"Container '{config.Value.ContainerName}' does not exist.");
                }

                return new CosmosDbService(container);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
