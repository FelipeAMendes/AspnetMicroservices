using Discount.Grpc.DependencyInjection;
using Discount.Grpc.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Discount.Grpc
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddGrpc();
			services.AddDiscountGrpcDependencies();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapGrpcService<DiscountService>();

				endpoints.MapGet("/", async context =>
					{
						await context.Response.WriteAsync(
							"Communication with gRPC endpoints must be made through a gRPC client.");
					});
			});
		}
	}
}
