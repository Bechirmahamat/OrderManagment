using Api.Dtos;
using Core.Features.CompanyManagement.Commands.Create;
using Core.Features.CompanyManagement.Queries.GetAll;
using Core.Features.CompanyManagement.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ISender mediator;

        public CompanyController(ISender mediator)
        {
            this.mediator = mediator;
        }
        // GET: api/company

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var response = await mediator.Send(new GetAllCompanyQuery());
            if (response.Success == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await mediator.Send(new GetCompnayByIdQuery(id));
            if (response.Success == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Post([FromForm] CreateCompanyRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            var response = await mediator.Send(new CreateCompanyCommand(request.Name, request.Description, request.ManagerId));
            if (response.Success == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
