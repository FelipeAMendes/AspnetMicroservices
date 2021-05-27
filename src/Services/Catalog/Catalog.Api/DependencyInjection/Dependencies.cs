using Catalog.Api.Configs;
using Catalog.Api.Data;
using Catalog.Api.Data.Interfaces;
using Catalog.Api.Repositories;
using Catalog.Api.Repositories.Interfaces;
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
		public static void AddCatalogApiDependencies(this IServiceCollection services)
		{
			services.AddSingleton<IDatabaseSettings>(p => p.GetRequiredService<IOptions<DatabaseSettings>>().Value);
			services.AddScoped<ICatalogContext, CatalogContext>();
			services.AddScoped<IProductRepository, ProductRepository>();
		}

		public static void ConfigureSwagger(this IServiceCollection services)
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
		}

		private static string GetXmlCommentsPath()
		{
			var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
			var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
			return xmlPath;
		}
	}
}
