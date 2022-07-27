using AuthR.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthR.DataAccess.EntityBuilders;

public static class RefreshTokenEntityBuilder
{
    public static void Build(this EntityTypeBuilder<RefreshTokenEntity> entityBuilder)
    {
        entityBuilder.ToTable("RefreshToken1");
        entityBuilder.HasKey(x => x.Id);
        
        entityBuilder.BuildIndexes();
        entityBuilder.BuildProperties();
    }

    private static void BuildIndexes(this EntityTypeBuilder<RefreshTokenEntity> entityBuilder)
    {
        entityBuilder.HasIndex(x => x.Guid)
            .IsUnique();
    }

    private static void BuildProperties(this EntityTypeBuilder<RefreshTokenEntity> entityBuilder)
    {
        entityBuilder.Property(x => x.Guid)
            .IsRequired()
            .HasDefaultValueSql("NEWID()")
            .ValueGeneratedOnAdd();
    }
}