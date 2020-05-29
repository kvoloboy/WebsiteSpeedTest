using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RequestSpeedTest.Domain.Entities;

namespace WebsiteSpeedTest.DataAccess.Configurations
{
    public class EndpointConfiguration : IEntityTypeConfiguration<Endpoint>
    {
        public void Configure(EntityTypeBuilder<Endpoint> builder)
        {
            builder.Property(request => request.Id).ValueGeneratedOnAdd();

            builder.HasOne(endpoint => endpoint.RequestBenchmarkEntry)
                .WithMany(request => request.Endpoints)
                .HasForeignKey(endpoint => endpoint.RequestBenchmarkEntryId);
        }
    }
}
