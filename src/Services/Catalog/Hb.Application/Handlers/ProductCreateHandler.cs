using AutoMapper;
using Hb.Application.Commands.ProductCreate;
using Hb.Application.Responses;
using Hb.Domain.Entities;
using Hb.Domain.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hb.Application.Handlers
{
    public class ProductCreateHandler : IRequestHandler<ProductCreateCommand, ProductResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductCreateHandler(
            IProductRepository productRepository,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductResponse> Handle(ProductCreateCommand request, CancellationToken cancellationToken)
        {
            var productEntity = _mapper.Map<Product>(request);
            if (productEntity == null)
                throw new ApplicationException("Entity could not be mapped!");

            var product = await _productRepository.AddAsync(productEntity, 5);

            var productResponse = _mapper.Map<ProductResponse>(product);

            return productResponse;
        }
    }
}
