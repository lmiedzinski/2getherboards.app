using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TogetherBoardsApp.Backend.Infrastructure.OutboxPattern;

namespace TogetherBoardsApp.Backend.Infrastructure.Database.EntityFramework.Configurations;

internal sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("outbox_messages");

        builder.HasKey(outboxMessage => outboxMessage.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .IsRequired();
        
        builder.Property(x => x.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired();
        
        builder.Property(x => x.Type)
            .HasColumnName("type")
            .IsRequired();
        
        builder.Property(outboxMessage => outboxMessage.Content)
            .HasColumnName("content")
            .HasColumnType("json")
            .IsRequired();

        builder.Property(x => x.ProcessedAtUtc)
            .HasColumnName("processed_at_utc");
        
        builder.Property(x => x.Error)
            .HasColumnName("error");
    }
}