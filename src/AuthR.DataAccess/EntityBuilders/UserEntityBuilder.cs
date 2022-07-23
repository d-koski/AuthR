using AuthR.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthR.DataAccess.EntityBuilders;

public static class UserEntityBuilder
{
    public static void Build(this EntityTypeBuilder<UserEntity> entityBuilder)
    {
        entityBuilder.ToTable("User");
        entityBuilder.HasKey(x => x.Id);
        
        entityBuilder.BuildIndexes();
        entityBuilder.BuildProperties();
    }

    private static void BuildIndexes(this EntityTypeBuilder<UserEntity> entityBuilder)
    {
        entityBuilder.HasIndex(x => x.Guid)
            .IsUnique();

        entityBuilder.HasIndex(x => x.Email)
            .IsUnique();
    }

    private static void BuildProperties(this EntityTypeBuilder<UserEntity> entityBuilder)
    {
        entityBuilder.Property(x => x.Guid)
            .ValueGeneratedOnAdd();
        
        entityBuilder.Property(x => x.Email)
            .HasMaxLength(320);

        entityBuilder.Property(x => x.PasswordHash)
            .HasMaxLength(256);
    }
}