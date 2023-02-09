using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WorkTool.Console.Configurations;

public class TimerItemEntityTypeConfiguration : IEntityTypeConfiguration<TimerItem>
{
    public void Configure(EntityTypeBuilder<TimerItem> builder)
    {
        builder.ToTable("Timers");
        builder.HasKey(x => x.Id);
    }
}
