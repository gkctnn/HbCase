using MediatR;

namespace Hb.Application.Queries
{
    public class DeleteCategoryByIdQuery : IRequest<bool>
    {
        public string Id { get; set; }

        public DeleteCategoryByIdQuery(string id)
        {
            Id = id;
        }
    }
}
