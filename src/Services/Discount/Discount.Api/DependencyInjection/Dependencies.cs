using Discount.Api.Repositories;
using Discount.Api.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace Discount.Api.DependencyInjection
{
	public static class Dependencies
	{
		public static void AddDiscountApiDependencies(this IServiceCollection services)
		{
			services.AddScoped<IDiscountRepository, DiscountRepository>();
		}

		public static void ConfigureSwagger(this IServiceCollection services)
		{
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "Discount.API",
					Version = "v1",
					Description = "Discount.API Microservice"
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
