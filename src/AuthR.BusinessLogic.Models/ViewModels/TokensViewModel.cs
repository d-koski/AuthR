namespace AuthR.BusinessLogic.Models.ViewModels;

public class TokensViewModel
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}