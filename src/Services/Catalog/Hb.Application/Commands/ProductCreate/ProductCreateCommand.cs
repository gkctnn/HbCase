using Hb.Application.Responses;
using MediatR;

namespace Hb.Application.Commands.ProductCreate
{
    public class ProductCreateCommand : IRequest<ProductResponse>
    {
        public string CategoryId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
    }
}
