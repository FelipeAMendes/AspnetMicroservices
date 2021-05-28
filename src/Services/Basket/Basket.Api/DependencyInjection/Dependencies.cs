using Basket.Api.Repositories;
using Basket.Api.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace Basket.Api.DependencyInjection
{
	public static class Dependencies
	{
		public static void AddBasketApiDependencies(this IServiceCollection services)
		{
			services.AddScoped<IBasketRepository, BasketRepository>();
		}

		public static void ConfigureSwagger(this IServiceCollection services)
		{
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "Basket.API",
					Version = "v1",
					Description = "Basket.API Microservice"
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
