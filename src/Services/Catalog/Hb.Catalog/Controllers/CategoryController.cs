using Hb.Application.Commands.CategoryCreate;
using Hb.Application.Queries;
using Hb.Application.Responses;
using Hb.Domain.Entities;
using Hb.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Hb.Catalog.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        #region Variables
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMediator _mediator;
        private readonly ILogger<CategoryController> _logger;
        #endregion

        #region Constructor
        public CategoryController(ICategoryRepository categoryRepository
            , ILogger<CategoryController> logger
            , IMediator mediator)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        #endregion

        #region Crud Actions
        [HttpGet]
        [ProducesResponseType(typeof(CategoryResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetCategories()
        {
            var query = new GetCategoriesQuery();

            var categories = await _mediator.Send(query);

            return Ok(categories);
        }

        [HttpGet("{id:length(24)}", Name = "GetCategory")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(CategoryResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<CategoryResponse>> GetCategory(string id)
        {
            var query = new GetCategoryByIdQuery(id);

            var category = await _mediator.Send(query);

            if (category == null)
            {
                _logger.LogError($"Category with id: {id}, not found.");

                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CategoryResponse), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<CategoryResponse>> CreateCategory([FromBody] CategoryCreateCommand command)
        {
            var result = await _mediator.Send(command);

            return CreatedAtRoute("GetCategory", new { id = result.Id }, command);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Category), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateCategory([FromBody] Category product)
        {
            return Ok(await _categoryRepository.UpdateAsync(product, 5));
        }


        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteCategoryById(string id)
        {
            var query = new DeleteCategoryByIdQuery(id);

            return Ok(await _mediator.Send(query));
        }
        #endregion
    }
}
