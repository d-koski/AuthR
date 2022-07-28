namespace AuthR.DataAccess.Entities;

public class RefreshTokenEntity : IBaseEntity, IPublicEntity
{
    public int Id { get; set; }
    
    public Guid Guid { get; set; }
    
    public DateTime Created { get; set; }
    
    public DateTime Modified { get; set; }

    public DateTime? Revoked { get; set; }
}