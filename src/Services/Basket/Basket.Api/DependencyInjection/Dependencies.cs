using Basket.Api.GrpcServices;
using Basket.Api.Repositories;
using Basket.Api.Repositories.Interfaces;
using Discount.Grpc.Protos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace Basket.Api.DependencyInjection
{
	public static class Dependencies
	{
		public static void AddBasketApiDependencies(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddScoped<IBasketRepository, BasketRepository>();
			services.AddScoped<DiscountGrpcService>();
			
			services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(opt =>
			{
				opt.Address = new Uri(configuration.GetValue<string>("GrpcSettings:DiscountUrl"));
			});

			services.AddStackExchangeRedisCache(opt =>
			{
				opt.Configuration = configuration.GetValue<string>("CacheSettings:ConnectionString");
			});
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
