using AuthR.Common.Abstractions.Services;
using AuthR.DataAccess.Entities;
using AuthR.DataAccess.EntityBuilders;
using Microsoft.EntityFrameworkCore;

namespace AuthR.DataAccess;

public class DatabaseContext : DbContext
{
    private readonly IDateTimeService _dateTimeService;

    public DatabaseContext(DbContextOptions<DatabaseContext> options, IDateTimeService dateTimeService) : base(options)
    {
        _dateTimeService = dateTimeService;
    }

    public DbSet<UserEntity> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RefreshTokenEntity>().Build();
        modelBuilder.Entity<UserEntity>().Build();
    }

    public override int SaveChanges()
    {
        UpdateAddedEntities();
        UpdateUpdatedEntities();
        return base.SaveChanges();
    }

    private void UpdateAddedEntities()
    {
        var addedEntities = ChangeTracker.Entries<IBaseEntity>()
            .Where(x => x.State == EntityState.Added)
            .Select(x => x.Entity);
        
        foreach (var entity in addedEntities)
        {
            var now = _dateTimeService.Now;
            entity.Created = now;
            entity.Modified = now;
        }
    }

    private void UpdateUpdatedEntities()
    {
        var updatedEntities = ChangeTracker.Entries<IBaseEntity>()
            .Where(x => x.State == EntityState.Modified)
            .Select(x => x.Entity);
        
        foreach (var entity in updatedEntities)
            entity.Modified = _dateTimeService.Now;
    }
}