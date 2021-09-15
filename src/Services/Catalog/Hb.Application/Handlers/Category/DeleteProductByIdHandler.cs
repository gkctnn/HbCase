using AutoMapper;
using Hb.Application.Queries;
using Hb.Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Handlers
{
    public class DeleteCategoryByIdHandler : IRequestHandler<DeleteCategoryByIdQuery, bool>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public DeleteCategoryByIdHandler(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(DeleteCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _categoryRepository.DeleteAsync(request.Id);

            var response = _mapper.Map<bool>(result);

            return response;
        }
    }
}
