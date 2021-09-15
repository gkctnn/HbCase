using Hb.Application.Commands.ProductCreate;
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
    public class ProductController : ControllerBase
    {
        #region Variables
        private readonly IProductRepository _productRepository;
        private readonly IMediator _mediator;
        private readonly ILogger<ProductController> _logger;
        #endregion

        #region Constructor
        public ProductController(IProductRepository productRepository
            , ILogger<ProductController> logger
            , IMediator mediator)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        #endregion

        #region Crud Actions
        [HttpGet]
        [ProducesResponseType(typeof(ProductResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProducts()
        {
            var query = new GetProductsQuery();

            var products = await _mediator.Send(query);

            return Ok(products);
        }

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProductResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ProductResponse>> GetProduct(string id)
        {
            var query = new GetProductByIdQuery(id);

            var product = await _mediator.Send(query);

            if (product == null)
            {
                _logger.LogError($"Product with id: {id}, not found.");

                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProductResponse), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<ProductResponse>> CreateProduct([FromBody] ProductCreateCommand command)
        {
            var result = await _mediator.Send(command);

            return CreatedAtRoute("GetProduct", new { id = result.Id }, command);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            return Ok(await _productRepository.UpdateAsync(product, 5));
        }


        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProductById(string id)
        {
            var query = new DeleteProductByIdQuery(id);

            return Ok(await _mediator.Send(query));
        }
        #endregion
    }
}
