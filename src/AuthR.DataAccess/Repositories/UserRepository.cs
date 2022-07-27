using AuthR.DataAccess.Abstractions.Repositories;
using AuthR.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthR.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DatabaseContext _databaseContext;

    public UserRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<bool> ExistsAsync(string email, CancellationToken cancellationToken)
    {
        var exists = await _databaseContext.Users.AnyAsync(x => x.Email == email, cancellationToken);
        return exists;
    }

    public async Task<UserEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var entity = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        return entity;
    }

    public async Task InsertAsync(UserEntity entity, CancellationToken cancellationToken)
    {
        await _databaseContext.Users.AddAsync(entity, cancellationToken);
    }
}