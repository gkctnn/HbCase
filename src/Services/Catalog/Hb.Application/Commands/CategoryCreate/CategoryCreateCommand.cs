using Hb.Application.Responses;
using MediatR;

namespace Hb.Application.Commands.CategoryCreate
{
    public class CategoryCreateCommand : IRequest<CategoryResponse>
    {
        public string Name { get; set; }
    }
}
