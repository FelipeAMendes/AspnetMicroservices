using Catalog.Api.Configs;
using Catalog.Api.Data;
using Catalog.Api.Data.Interfaces;
using Catalog.Api.Repositories;
using Catalog.Api.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace Catalog.Api.DependencyInjection
{
	public static class Dependencies
	{
		public static IServiceCollection AddCatalogApiDependencies(this IServiceCollection services, IConfiguration configuration)
		{
			return services
				.Configure<DatabaseSettings>(configuration.GetSection(nameof(DatabaseSettings)))
				.AddSingleton<IDatabaseSettings>(p => p.GetRequiredService<IOptions<DatabaseSettings>>().Value)
				.AddScoped<ICatalogContext, CatalogContext>()
				.AddScoped<IProductRepository, ProductRepository>();
		}

		public static IServiceCollection AddSwagger(this IServiceCollection services)
		{
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "Catalog.API",
					Version = "v1",
					Description = "Catalog.API Microservice"
				});

				c.IncludeXmlComments(GetXmlCommentsPath());
			});

			return services;
		}

		private static string GetXmlCommentsPath()
		{
			var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
			var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
			return xmlPath;
		}
	}
}
