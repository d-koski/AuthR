namespace AuthR.DataAccess.Entities;

public interface IBaseEntity
{
    public int Id { get; set; }

    public Guid Guid { get; set; }
    
    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }
}