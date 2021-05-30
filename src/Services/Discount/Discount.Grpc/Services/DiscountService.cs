using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories.Interfaces;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Discount.Grpc.Services
{
	public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
	{
		private readonly ILogger<DiscountService> _logger;
		private readonly IMapper _mapper;
		private readonly IDiscountRepository _discountRepository;

		public DiscountService(ILogger<DiscountService> logger, IMapper mapper, IDiscountRepository discountRepository)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_discountRepository = discountRepository ?? throw new ArgumentNullException(nameof(discountRepository));
		}

		public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
		{
			var coupon = await _discountRepository.Get(request.ProductName);
			if (coupon == null)
				throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName {request.ProductName} was not found."));

			_logger.LogInformation($"Discount is retrieved for ProductName {coupon.ProductName}, Amount {coupon.Amount}");

			var couponModel = _mapper.Map<CouponModel>(coupon);
			return couponModel;
		}

		public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
		{
			var coupon = _mapper.Map<Coupon>(request.Coupon);

			await _discountRepository.Create(coupon);
			_logger.LogInformation($"Discount is successfully created. ProductName {coupon.ProductName}");

			var couponModel = _mapper.Map<CouponModel>(coupon);
			return couponModel;
		}

		public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
		{
			var coupon = _mapper.Map<Coupon>(request.Coupon);

			await _discountRepository.Update(coupon);
			_logger.LogInformation($"Discount is successfully updated. ProductName {coupon.ProductName}");

			var couponModel = _mapper.Map<CouponModel>(coupon);
			return couponModel;
		}

		public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
		{
			var deleted = await _discountRepository.Delete(request.ProductName);
			var response = new DeleteDiscountResponse { Success = deleted };
			return response;
		}
	}
}
