
using Api.Dtos;
using Core.Features.AuthManagement.Commands.Login;
using Core.Features.AuthManagement.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ISender mediator;

        public AuthenticationController(ISender mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await mediator.Send(new RegisterUserCommand(request.FirstName, request.LastName, request.Email, request.Password));
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await mediator.Send(new LoginCommand(request.Email, request.Password));
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
