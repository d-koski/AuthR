using AuthR.BusinessLogic.Models.Commands;
using AuthR.BusinessLogic.Models.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthR.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<UserViewModel>> Register(
        [FromBody] RegisterUserCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result;
    }
}