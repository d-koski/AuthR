using AuthR.BusinessLogic.Models.ViewModels;
using MediatR;

namespace AuthR.BusinessLogic.Models.Commands;

public class AuthenticateUserCommand : IRequest<TokensViewModel>
{
    public string Email { get; set; }

    public string Password { get; set; }
}