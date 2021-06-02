using MediatR;
using System;
using System.Collections.Generic;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
	public class GetOrdersListQuery : IRequest<IEnumerable<OrdersViewModel>>
	{
		public string UserName { get; set; }

		public GetOrdersListQuery(string userName)
		{
			UserName = userName ?? throw new ArgumentNullException(nameof(userName));
		}
	}
}
