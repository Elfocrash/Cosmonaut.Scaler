using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Threading.Tasks;
using Cosmonaut.Scaler.Server.Data;
using Cosmonaut.Scaler.Server.Services;
using Microsoft.Azure.Documents.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cosmonaut.Scaler.Server
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
            services.AddMvc().AddNewtonsoftJson();
            services.AddResponseCompression();
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            }, ServiceLifetime.Singleton);

            services.AddSingleton<ICosmosService, CosmosService>();
            services.AddSingleton(provider =>
            {
                var cosmosService = provider.GetRequiredService<ICosmosService>();

                var client = new CosmonautClientHolder();
                var accounts = cosmosService.GetAllCosmosAccounts().GetAwaiter().GetResult();

                foreach (var account in accounts)
                {
                    var cosmonautClient = new CosmonautClient(account.Endpoint, account.MasterKey,
                        new ConnectionPolicy { ConnectionMode = ConnectionMode.Direct, ConnectionProtocol = Protocol.Tcp });
                    client.AddClient(cosmonautClient);
                }

                return client;
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default", template: "{controller}/{action}/{id?}");
            });

            app.UseBlazor<Client.Startup>();
            app.UseBlazorDebugging();
        }
    }
}
