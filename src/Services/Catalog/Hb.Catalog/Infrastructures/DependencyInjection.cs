using Hb.Catalog.Infrastructures.Data;
using Hb.Catalog.Infrastructures.Data.Interfaces;
using Hb.Catalog.Infrastructures.Settings;
using Hb.Catalog.Infrastructures.Repositories;
using Hb.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Hb.Domain.Repositories.Base;
using Hb.Catalog.Infrastructures.Repositories.Base;
using Hb.Domain.Entities;

namespace Hb.Catalog.Infrastructures
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region Swagger Dependencies
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hb.Catalog", Version = "v1" });
            });
            #endregion

            #region Configuration Dependencies
            services.Configure<DatabaseSettings>(configuration.GetSection(nameof(DatabaseSettings)));
            services.AddSingleton<IDatabaseSettings>(sp => sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);
            #endregion

            #region Project Dependencies
            services.AddTransient(typeof(IRepository<Product>), typeof(ProductRepository));
            services.AddTransient<ICatalogContext, CatalogContext>();
            services.AddTransient<IProductRepository, ProductRepository>();
            #endregion

            #region Redis Dependencies
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetValue<string>("CacheSettings:ConnectionString");
            });
            #endregion

            return services;
        }
    }
}
