using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace Ordering.Api.DependencyInjection
{
	public static class Dependencies
	{
		public static IServiceCollection AddSwagger(this IServiceCollection services)
		{
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "Ordering.API",
					Version = "v1",
					Description = "Ordering.API Microservice"
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
