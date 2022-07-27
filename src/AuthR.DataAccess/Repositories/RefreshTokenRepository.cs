using AuthR.DataAccess.Abstractions.Repositories;
using AuthR.DataAccess.Entities;

namespace AuthR.DataAccess.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly DatabaseContext _databaseContext;

    public RefreshTokenRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }
    
    public async Task<RefreshTokenEntity> GenerateAsync(CancellationToken cancellationToken)
    {
        var entity = new RefreshTokenEntity();
        await _databaseContext.AddAsync(entity, cancellationToken);
        return entity;
    }
}