using AutoMapper;
using Hb.Application.Queries;
using Hb.Application.Responses;
using Hb.Domain.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hb.Application.Handlers
{
    public class GetProductsHandlers : IRequestHandler<GetProductsQuery, IEnumerable<ProductResponse>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductsHandlers(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductResponse>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllAsync(p => true);

            var response = _mapper.Map<IEnumerable<ProductResponse>>(products);

            return response;
        }
    }
}
