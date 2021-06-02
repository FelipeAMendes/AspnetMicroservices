using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
	public class GetOrdersListQueryHandler : IRequestHandler<GetOrdersListQuery, IEnumerable<OrdersViewModel>>
	{
		private readonly IOrderRepository _orderRepository;
		private readonly IMapper _mapper;

		public GetOrdersListQueryHandler(IOrderRepository orderRepository, IMapper mapper)
		{
			_orderRepository = orderRepository;
			_mapper = mapper;
		}

		public async Task<IEnumerable<OrdersViewModel>> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
		{
			var orderList = await _orderRepository.GetOrdersByUserName(request.UserName);
			return _mapper.Map<IEnumerable<OrdersViewModel>>(orderList);
		}
	}
}
