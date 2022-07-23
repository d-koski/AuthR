namespace AuthR.DataAccess.Entities;

public class UserEntity : IBaseEntity, IPublicEntity
{
    public int Id { get; set; }
    
    public Guid Guid { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;
}