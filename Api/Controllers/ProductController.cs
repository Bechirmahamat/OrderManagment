using System.Security.Claims;
using Api.Dtos;
using Core.Features.ProductManagement.Commands.Create;
using Core.Features.ProductManagement.Queries.GetAll;
using Core.Features.ProductManagement.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ISender mediator;

        public ProductController(ISender mediator)
        {
            this.mediator = mediator;
        }
        // GET: api/product

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var response = await mediator.Send(new GetAllProductQuery());
            if (response.Success == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await mediator.Send(new GetProductByIdQuery(id));
            if (response.Success == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "MANAGER")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductRequest request)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ///get maneger id form the token
            ///

            var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await mediator.Send(new CreateProductCommand(request.Name, request.Description, request.Price, request.StockCount, request.CategoryId, managerId));
            if (response.Success == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
