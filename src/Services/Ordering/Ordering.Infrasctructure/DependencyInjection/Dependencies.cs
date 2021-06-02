using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Infrastructure.Mail;
using Ordering.Infrastructure.Persistence;
using Ordering.Infrastructure.Repositories;

namespace Ordering.Infrastructure.DependencyInjection
{
	public static class Dependencies
	{
		public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<OrderContext>(options =>
				options.UseSqlServer(configuration.GetConnectionString("OrderingConnectionString")));

			return services
				.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>))
				.AddScoped<IOrderRepository, OrderRepository>()
				.Configure<EmailSettings>(c => configuration.GetSection("EmailSettings"))
				.AddTransient<IEmailService, EmailService>();
		}
	}
}
