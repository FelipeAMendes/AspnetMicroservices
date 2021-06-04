using Basket.Api.GrpcServices;
using Basket.Api.Repositories;
using Basket.Api.Repositories.Interfaces;
using Discount.Grpc.Protos;
using MassTransit;
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
		public static IServiceCollection AddBasketApiDependencies(this IServiceCollection services, IConfiguration configuration)
		{
			// Redis Configuration
			services.AddStackExchangeRedisCache(opt
				=> opt.Configuration = configuration.GetValue<string>("CacheSettings:ConnectionString"));

			// General Configuration
			services.AddScoped<IBasketRepository, BasketRepository>();
			services.AddAutoMapper(typeof(Startup));

			// Grpc Configuration
			services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(opt
				=> opt.Address = new Uri(configuration["GrpcSettings:DiscountUrl"]));
			services.AddScoped<DiscountGrpcService>();

			// MassTransit-RabbitMQ Configuration
			services.AddMassTransit(config =>
			{
				config.UsingRabbitMq((ctx, cfg) =>
				{
					cfg.Host(configuration["EventBusSettings:HostAddress"]);
				});
			});

			return services;
		}

		public static IServiceCollection AddSwagger(this IServiceCollection services)
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
