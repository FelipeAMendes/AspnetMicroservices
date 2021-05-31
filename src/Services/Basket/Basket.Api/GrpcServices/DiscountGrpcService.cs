using Discount.Grpc.Protos;
using System;
using System.Threading.Tasks;

namespace Basket.Api.GrpcServices
{
	public class DiscountGrpcService
	{
		private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;

		public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
		{
			_discountProtoServiceClient = discountProtoServiceClient ?? throw new ArgumentNullException(nameof(discountProtoServiceClient));
		}

		public async Task<CouponModel> GetDiscount(string productName)
		{
			var discountRequest = new GetDiscountRequest { ProductName = productName };
			return await _discountProtoServiceClient.GetDiscountAsync(discountRequest);
		}

		public async Task<CouponModel> Create()
		{
			//TODO: Implement...
			var createRequest = new CreateDiscountRequest
			{
				Coupon = new CouponModel { }
			};

			return await _discountProtoServiceClient.CreateDiscountAsync(createRequest);
		}
	}
}
