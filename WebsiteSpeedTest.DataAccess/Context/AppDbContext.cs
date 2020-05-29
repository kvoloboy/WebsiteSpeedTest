﻿using Microsoft.EntityFrameworkCore;
using WebsiteSpeedTest.DataAccess.Configurations;

namespace WebsiteSpeedTest.DataAccess.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options)
            : base(options)
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RequestBenchmarkEntryConfiguration());
        }
    }
}
