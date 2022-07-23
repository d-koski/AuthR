using AuthR.Common.Abstractions.Services;
using AuthR.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthR.DataAccess.UnitTests;

public class DatabaseContextTests
{
    private readonly DatabaseContext _databaseContext;
    private readonly Mock<IDateTimeService> _dateTimeServiceMock = new();
    
    public DatabaseContextTests()
    {
        _databaseContext = GetDatabaseContext();
    }

    private DatabaseContext GetDatabaseContext()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new DatabaseContext(options, _dateTimeServiceMock.Object);
        context.Database.EnsureCreated();

        return context;
    }

    [Fact]
    public void SaveChanges_EntityAdded_SetsCreatedTime()
    {
        var currentTime = new DateTime(2022, 7, 23);
        _dateTimeServiceMock.Setup(x => x.Now)
            .Returns(currentTime);

        var entityToAdd = new UserEntity
        {
            Email = "",
            PasswordHash = "",
        };
        _databaseContext.Users.Add(entityToAdd);
        
        _databaseContext.SaveChanges();

        var addedEntity = _databaseContext.Users.First();
        Assert.Equal(currentTime, addedEntity.Created);
    }

    [Fact]
    public void SaveChanges_EntityAdded_SetsModifiedTime()
    {
        var currentTime = new DateTime(2022, 7, 23);
        _dateTimeServiceMock.Setup(x => x.Now)
            .Returns(currentTime);

        var entityToAdd = new UserEntity
        {
            Email = "",
            PasswordHash = "",
        };
        _databaseContext.Users.Add(entityToAdd);
        
        _databaseContext.SaveChanges();

        var addedEntity = _databaseContext.Users.First();
        Assert.Equal(currentTime, addedEntity.Modified);
    }

    [Fact]
    public void SaveChanges_EntityUpdated_SetsModifiedTime()
    {
        var createdTime = new DateTime(2022, 7, 23);
        var modifiedTime = new DateTime(2022, 7, 24);
        _dateTimeServiceMock.SetupSequence(x => x.Now)
            .Returns(createdTime)
            .Returns(modifiedTime);
        
        var entityToUpdate = new UserEntity
        {
            Email = "",
            PasswordHash = "",
        };
        _databaseContext.Users.Add(entityToUpdate);
        _databaseContext.SaveChanges();

        entityToUpdate.Email = "x@y.z";
        _databaseContext.Users.Update(entityToUpdate);
        _databaseContext.SaveChanges();
        
        var addedEntity = _databaseContext.Users.First();
        Assert.Equal(modifiedTime, addedEntity.Modified);
    }
}