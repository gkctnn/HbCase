using EventBusRabbitMQ;
using EventBusRabbitMQ.Producer;
using Hb.Catalog.Data;
using Hb.Catalog.Data.Interfaces;
using Hb.Catalog.Repositories;
using Hb.Catalog.Repositories.Interfaces;
using Hb.Catalog.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;

namespace Hb.Catalog
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

            services.AddControllers();

            #region Swagger Dependencies
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hb.Catalog", Version = "v1" });
            });
            #endregion

            #region Configuration Dependencies
            services.Configure<DatabaseSettings>(Configuration.GetSection(nameof(DatabaseSettings)));
            services.AddSingleton<IDatabaseSettings>(sp => sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);
            #endregion

            #region Project Dependencies
            services.AddTransient<ICatalogContext, CatalogContext>();
            services.AddTransient<IProductRepository, ProductRepository>();

            services.AddAutoMapper(typeof(Startup));
            #endregion

            #region EventBus

            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                var factory = new ConnectionFactory()
                {
                    HostName = Configuration["EventBus:HostName"]
                };

                if (!string.IsNullOrWhiteSpace(Configuration["EventBus:UserName"]))
                {
                    factory.UserName = Configuration["EventBus:UserName"];
                }

                if (!string.IsNullOrWhiteSpace(Configuration["EventBus:Password"]))
                {
                    factory.UserName = Configuration["EventBus:Password"];
                }

                var retryCount = 5;
                if (!string.IsNullOrWhiteSpace(Configuration["EventBus:RetryCount"]))
                {
                    retryCount = int.Parse(Configuration["EventBus:RetryCount"]);
                }

                return new DefaultRabbitMQPersistentConnection(factory, retryCount, logger);
            });

            services.AddSingleton<EventBusRabbitMQProducer>();

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hb.Catalog v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
