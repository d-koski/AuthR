using AuthR.Common.Abstractions.Services;
using AuthR.DataAccess.Abstractions.Repositories;
using AuthR.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthR.DataAccess.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly DatabaseContext _databaseContext;
    private readonly IDateTimeService _dateTimeService;

    public RefreshTokenRepository(DatabaseContext databaseContext, IDateTimeService dateTimeService)
    {
        _databaseContext = databaseContext;
        _dateTimeService = dateTimeService;
    }
    
    public async Task<RefreshTokenEntity> GenerateAsync(CancellationToken cancellationToken)
    {
        var entity = new RefreshTokenEntity();
        await _databaseContext.AddAsync(entity, cancellationToken);
        return entity;
    }

    public Task<RefreshTokenEntity?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken)
    {
        var entity = _databaseContext.RefreshTokens
            .FirstOrDefaultAsync(x => x.Guid == guid, cancellationToken);
        return entity;
    }

    public void Revoke(RefreshTokenEntity entity)
    {
        entity.Revoked ??= _dateTimeService.Now;
    }
}