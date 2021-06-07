using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Shopping.Aggregator.Services;
using System;
using System.IO;
using System.Reflection;

namespace Shopping.Aggregator.DependencyInjection
{
	public static class Dependencies
	{
		public static IServiceCollection AddShoppingAggregatorDependencies(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddHttpClient<ICatalogService, CatalogService>(c =>
				c.BaseAddress = new Uri(configuration["ApiSettings:CatalogUrl"]));

			services.AddHttpClient<IBasketService, BasketService>(c =>
				c.BaseAddress = new Uri(configuration["ApiSettings:BasketUrl"]));

			services.AddHttpClient<IOrderService, OrderService>(c =>
				c.BaseAddress = new Uri(configuration["ApiSettings:OrderingUrl"]));

			return services;
		}

		public static IServiceCollection AddSwagger(this IServiceCollection services)
		{
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "Shopping.Aggregator",
					Version = "v1",
					Description = "Shopping Aggregator Microservice"
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
