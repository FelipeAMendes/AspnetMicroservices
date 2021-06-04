using EventBus.Messages.Common;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Ordering.Api.EventBusConsumer;
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

		public static IServiceCollection AddOrderingApiDependencies(this IServiceCollection services, IConfiguration configuration)
		{
			// MassTransit-RabbitMQ Configuration
			services.AddMassTransit(config =>
			{
				config.AddConsumer<BasketCheckoutConsumer>();

				config.UsingRabbitMq((ctx, cfg) =>
				{
					cfg.Host(configuration["EventBusSettings:HostAddress"]);
					cfg.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c =>
					{
						c.ConfigureConsumer<BasketCheckoutConsumer>(ctx);
					});
				});
			});
			services.AddMassTransitHostedService();

			// General Configuration
			services.AddScoped<BasketCheckoutConsumer>();
			services.AddAutoMapper(typeof(Startup));

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
