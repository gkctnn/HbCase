using Hb.Application.Responses;
using MediatR;
using System.Collections.Generic;

namespace Hb.Application.Queries
{
     public class GetCategoriesQuery : IRequest<IEnumerable<CategoryResponse>>
    {
        public GetCategoriesQuery()
        {

        }
    }
}
