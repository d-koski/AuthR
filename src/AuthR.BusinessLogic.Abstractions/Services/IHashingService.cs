namespace AuthR.BusinessLogic.Abstractions.Services;

public interface IHashingService
{
    string HashPassword(string password);
    
    /// <summary>
    /// Checks if a password matches a hashed password.
    /// </summary>
    /// <param name="passwordHash">Password hash to verify <see cref="password" /> against.</param>
    /// <param name="password">Password to be verified.</param>
    bool VerifyPassword(string passwordHash, string password);
}