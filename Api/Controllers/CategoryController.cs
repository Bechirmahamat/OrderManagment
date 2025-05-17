using Api.Dtos;
using Core.Features.CategoryManagement.Commands.Create;
using Core.Features.CategoryManagement.Queries.GetAll;
using Core.Features.CategoryManagement.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ISender mediator;

        public CategoryController(ILogger<CategoryController> logger, ISender mediator)
        {
            _logger = logger;
            this.mediator = mediator;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var response = await mediator.Send(new GetAllCategoryQuery());
            if (response.Success == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await mediator.Send(new GetCategoryByIdQuery(id));
            if (response.Success == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPost]

        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Post([FromForm] CategoryRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ///
            var response = await mediator.Send(new CreateCategoryCommand(request.Name, request.Description));
            if (response.Success == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] string category)
        {
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return NoContent();
        }
    }
}
