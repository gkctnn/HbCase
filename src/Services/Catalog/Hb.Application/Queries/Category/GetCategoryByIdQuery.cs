using Hb.Application.Responses;
using MediatR;

namespace Hb.Application.Queries
{
    public class GetCategoryByIdQuery : IRequest<CategoryResponse>
    {
        public string Id { get; set; }

        public GetCategoryByIdQuery(string id)
        {
            Id = id;
        }
    }
}
