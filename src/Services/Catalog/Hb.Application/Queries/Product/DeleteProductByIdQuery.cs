using MediatR;

namespace Hb.Application.Queries
{
    public class DeleteProductByIdQuery : IRequest<bool>
    {
        public string Id { get; set; }

        public DeleteProductByIdQuery(string id)
        {
            Id = id;
        }
    }
}
