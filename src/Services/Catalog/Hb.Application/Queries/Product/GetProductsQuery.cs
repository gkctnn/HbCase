using Hb.Application.Responses;
using MediatR;
using System.Collections.Generic;

namespace Hb.Application.Queries
{
     public class GetProductsQuery : IRequest<IEnumerable<ProductResponse>>
    {
        public GetProductsQuery()
        {

        }
    }
}
