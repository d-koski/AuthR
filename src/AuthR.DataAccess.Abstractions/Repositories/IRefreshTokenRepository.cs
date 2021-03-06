using AuthR.DataAccess.Entities;

namespace AuthR.DataAccess.Abstractions.Repositories;

public interface IRefreshTokenRepository
{
    Task<RefreshTokenEntity> GenerateAsync(CancellationToken cancellationToken);

    Task<RefreshTokenEntity?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken);

    void Revoke(RefreshTokenEntity entity);
}