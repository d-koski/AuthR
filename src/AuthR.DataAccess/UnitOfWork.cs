using AuthR.DataAccess.Abstractions;

namespace AuthR.DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly DatabaseContext _databaseContext;

    public UnitOfWork(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _databaseContext.SaveChangesAsync(cancellationToken);
    }
}