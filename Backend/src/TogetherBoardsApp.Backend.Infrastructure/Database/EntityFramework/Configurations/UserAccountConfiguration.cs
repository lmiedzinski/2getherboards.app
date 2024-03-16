using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TogetherBoardsApp.Backend.Domain.UserAccounts;

namespace TogetherBoardsApp.Backend.Infrastructure.Database.EntityFramework.Configurations;

internal sealed class UserAccountConfiguration : IEntityTypeConfiguration<UserAccount>
{
    public void Configure(EntityTypeBuilder<UserAccount> builder)
    {
        builder.ToTable("user_accounts");

        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .IsRequired()
            .HasConversion(
                userAccountId => userAccountId.Value,
                value => new UserAccountId(value));
        
        builder.Property(x => x.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(128)
            .HasConversion(
                name => name.Value,
                value => new UserAccountName(value));
        
        builder.Property(x => x.Email)
            .HasColumnName("email")
            .IsRequired()
            .HasMaxLength(256)
            .HasConversion(
                email => email.Value,
                value => new UserAccountEmail(value));
        
        builder.Property(x => x.PasswordHash)
            .HasColumnName("password_hash")
            .IsRequired()
            .HasMaxLength(512)
            .HasConversion(
                passwordHash => passwordHash.Value,
                value => new UserAccountPasswordHash(value));

        builder.OwnsOne(x => x.RefreshToken, refreshTokenBuilder =>
        {
            refreshTokenBuilder.Property(rt => rt.Value)
                .HasColumnName("refresh_token_value")
                .HasMaxLength(1024);
            
            refreshTokenBuilder.Property(rt => rt.ExpirationDateUtc)
                .HasColumnName("refresh_token_expiration_date_utc");
            
            refreshTokenBuilder.Property(rt => rt.IsActive)
                .HasColumnName("refresh_token_is_active");
        });

        builder.Property(x => x.IsDeleted)
            .HasColumnName("is_deleted")
            .IsRequired();
        
        builder.Property<uint>("version")
            .IsRowVersion();

        builder.HasIndex(x => x.Email)
            .IsUnique();
    }
}