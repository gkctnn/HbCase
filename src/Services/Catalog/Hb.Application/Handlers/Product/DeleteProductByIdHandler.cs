using AutoMapper;
using Hb.Application.Queries;
using Hb.Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Handlers
{
    public class DeleteProductByIdHandler : IRequestHandler<DeleteProductByIdQuery, bool>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public DeleteProductByIdHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(DeleteProductByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _productRepository.DeleteAsync(request.Id);

            var response = _mapper.Map<bool>(result);

            return response;
        }
    }
}
