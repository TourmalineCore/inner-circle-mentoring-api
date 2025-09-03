using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Mappings;

public class OneOnOnesMapping : IEntityTypeConfiguration<OneOnOne>
{
    public void Configure(EntityTypeBuilder<OneOnOne> builder)
    {
        builder
            .Property(e => e.Note)
            .IsRequired();
    }
}