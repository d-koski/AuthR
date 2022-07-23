using AuthR.BusinessLogic.Models.ViewModels;
using MediatR;

namespace AuthR.BusinessLogic.Models.Commands;

public class RegisterUserCommand : IRequest<UserViewModel>
{
    public string Email { get; set; }
    
    public string Password { get; set; }
}