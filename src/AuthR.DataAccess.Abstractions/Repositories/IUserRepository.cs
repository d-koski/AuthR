using AuthR.DataAccess.Entities;

namespace AuthR.DataAccess.Abstractions.Repositories;

public interface IUserRepository
{
    /// <summary>
    /// Checks if a user with the given email already exists in the database.
    /// </summary>
    Task<bool> ExistsAsync(string email, CancellationToken cancellationToken);
    
    Task<UserEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    
    Task InsertAsync(UserEntity entity, CancellationToken cancellationToken);
}