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
    public class GetCategoriesHandlers : IRequestHandler<GetCategoriesQuery, IEnumerable<CategoryResponse>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public GetCategoriesHandlers(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryResponse>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetAllAsync(p => true);

            var response = _mapper.Map<IEnumerable<CategoryResponse>>(categories);

            return response;
        }
    }
}
