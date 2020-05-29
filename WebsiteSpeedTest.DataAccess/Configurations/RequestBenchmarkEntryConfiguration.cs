using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RequestSpeedTest.Domain.Entities;

namespace WebsiteSpeedTest.DataAccess.Configurations
{
    public class RequestBenchmarkEntryConfiguration : IEntityTypeConfiguration<RequestBenchmarkEntry>
    {
        public void Configure(EntityTypeBuilder<RequestBenchmarkEntry> builder)
        {
            builder.Property(r => r.Id).ValueGeneratedOnAdd();

            builder.HasMany(request => request.Endpoints)
                .WithOne(endpoint => endpoint.RequestBenchmarkEntry)
                .HasForeignKey(endpoint => endpoint.RequestBenchmarkEntryId);
        }
    }
}
