using AutoMapper;
using Hb.Application.Commands.CategoryCreate;
using Hb.Application.Responses;
using Hb.Domain.Entities;
using Hb.Domain.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hb.Application.Handlers
{
    public class CategoryCreateHandler : IRequestHandler<CategoryCreateCommand, CategoryResponse>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryCreateHandler(
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<CategoryResponse> Handle(CategoryCreateCommand request, CancellationToken cancellationToken)
        {
            var categoryEntity = _mapper.Map<Category>(request);
            if (categoryEntity == null)
                throw new ApplicationException("Entity could not be mapped!");

            var category = await _categoryRepository.AddAsync(categoryEntity, 5);

            var categoryResponse = _mapper.Map<CategoryResponse>(category);

            return categoryResponse;
        }
    }
}
