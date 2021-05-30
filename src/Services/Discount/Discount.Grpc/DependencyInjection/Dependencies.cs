using Discount.Grpc.Repositories;
using Discount.Grpc.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Discount.Grpc.DependencyInjection
{
	public static class Dependencies
	{
		public static void AddDiscountGrpcDependencies(this IServiceCollection services)
		{
			services.AddAutoMapper(typeof(Startup));
			services.AddScoped<IDiscountRepository, DiscountRepository>();
		}
	}
}
