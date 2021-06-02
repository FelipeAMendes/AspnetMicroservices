using Discount.Grpc.Repositories;
using Discount.Grpc.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Discount.Grpc.DependencyInjection
{
	public static class Dependencies
	{
		public static IServiceCollection AddDiscountGrpcDependencies(this IServiceCollection services)
		{
			return services
				.AddAutoMapper(typeof(Startup))
				.AddScoped<IDiscountRepository, DiscountRepository>();
		}
	}
}
