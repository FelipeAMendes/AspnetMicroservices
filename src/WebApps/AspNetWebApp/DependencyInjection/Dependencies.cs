using AspNetWebApp.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Serilog;
using System;
using System.Net.Http;

namespace AspNetWebApp.DependencyInjection
{
	public static class Dependencies
	{
		public static IServiceCollection AddAspNetWebAppDependencies(this IServiceCollection services, IConfiguration configuration)
		{
			//services.AddTransient<LoggingDelegatingHandler>();

			services.AddHttpClient<ICatalogService, CatalogService>(c =>
				c.BaseAddress = new Uri(configuration["ApiSettings:GatewayAddress"]))
				//.AddHttpMessageHandler<LoggingDelegatingHandler>()
				.AddPolicyHandler(GetRetryPolicy())
				.AddPolicyHandler(GetCircuitBreakerPolicy());

			services.AddHttpClient<IBasketService, BasketService>(c =>
				c.BaseAddress = new Uri(configuration["ApiSettings:GatewayAddress"]))
				//.AddHttpMessageHandler<LoggingDelegatingHandler>()
				.AddPolicyHandler(GetRetryPolicy())
				.AddPolicyHandler(GetCircuitBreakerPolicy());

			services.AddHttpClient<IOrderService, OrderService>(c =>
				c.BaseAddress = new Uri(configuration["ApiSettings:GatewayAddress"]))
				//.AddHttpMessageHandler<LoggingDelegatingHandler>()
				.AddPolicyHandler(GetRetryPolicy())
				.AddPolicyHandler(GetCircuitBreakerPolicy());

			return services;
		}

		private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
		{
			// In this case will wait for
			//  2 ^ 1 = 2 seconds then
			//  2 ^ 2 = 4 seconds then
			//  2 ^ 3 = 8 seconds then
			//  2 ^ 4 = 16 seconds then
			//  2 ^ 5 = 32 seconds

			return HttpPolicyExtensions
				.HandleTransientHttpError()
				.WaitAndRetryAsync(
					retryCount: 5,
					sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
					onRetry: (exception, retryCount, context) =>
					{
						Log.Error($"Retry {retryCount} of {context.PolicyKey} at {context.OperationKey}, due to: {exception}.");
					});
		}

		private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
		{
			return HttpPolicyExtensions
				.HandleTransientHttpError()
				.CircuitBreakerAsync(
						handledEventsAllowedBeforeBreaking: 5,
						durationOfBreak: TimeSpan.FromSeconds(30)
				);
		}
	}
}
