using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Behaviours;
using System.Reflection;

namespace Ordering.Application.DependencyInjection
{
	public static class Dependencies
	{
		public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
		{
			return services
				.AddAutoMapper(Assembly.GetExecutingAssembly())
				.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
				.AddMediatR(Assembly.GetExecutingAssembly())
				.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>))
				.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
		}
	}
}
