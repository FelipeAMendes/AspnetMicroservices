using Dapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Threading.Tasks;

namespace Discount.Grpc.Repositories
{
	/// <summary>
	/// TODO: NpgsqlConnection
	/// </summary>
	public class DiscountRepository : IDiscountRepository
	{
		private readonly IConfiguration _configuration;
		private readonly string _connectionString;

		public DiscountRepository(IConfiguration configuration)
		{
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
			_connectionString = _configuration.GetValue<string>("DatabaseSettings:ConnectionString");
		}

		public async Task<Coupon> Get(string productName)
		{
			using var connection = new NpgsqlConnection(_connectionString);

			var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(
				"SELECT * FROM Coupon WHERE ProductName = @ProductName", new { @ProductName = productName });

			return coupon ?? new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };
		}

		public async Task<bool> Create(Coupon coupon)
		{
			using var connection = new NpgsqlConnection(_connectionString);

			var affected = await connection.ExecuteAsync(
				@"INSERT INTO Coupon
						(ProductName, Description, Amount)
					VALUES
						(@ProductName, @Description, @Amount)",
				new
				{
					coupon.ProductName,
					coupon.Description,
					coupon.Amount
				});

			return affected != 0;
		}

		public async Task<bool> Update(Coupon coupon)
		{
			using var connection = new NpgsqlConnection(_connectionString);

			var affected = await connection.ExecuteAsync(
				@"UPDATE Coupon
					SET ProductName = @ProductName, Description = @Description, Amount = @Amount
					WHERE Id = @Id",
				new
				{
					coupon.ProductName,
					coupon.Description,
					coupon.Amount,
					coupon.Id
				});

			return affected != 0;
		}

		public async Task<bool> Delete(string productName)
		{
			using var connection = new NpgsqlConnection(_connectionString);

			var affected = await connection.ExecuteAsync(
				"DELETE FROM Coupon WHERE ProductName = @ProductName",
				new { ProductName = productName });

			return affected != 0;
		}
	}
}
