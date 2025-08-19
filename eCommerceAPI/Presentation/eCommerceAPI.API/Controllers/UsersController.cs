using eCommerceAPI.Application.Features.AppUser.Commands.CreateUser;
using eCommerceAPI.Application.Features.AppUser.Commands.FacebookLogin;
using eCommerceAPI.Application.Features.AppUser.Commands.GoogleLogin;
using eCommerceAPI.Application.Features.AppUser.Commands.LoginUser;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserCommandRequest createUserCommandRequest)
        {
            CreateUserCommandResponse response = await _mediator.Send(createUserCommandRequest);

            return Ok(response);
        }

        
    }
}
