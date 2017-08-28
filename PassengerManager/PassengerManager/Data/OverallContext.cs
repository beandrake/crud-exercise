﻿using PassengerManager.Models;
using Microsoft.EntityFrameworkCore;

namespace PassengerManager.Data
{
    public class OverallContext : DbContext
    {
        public OverallContext(DbContextOptions<OverallContext> options) : base(options)
        {
        }

        public DbSet<Passenger> Passengers { get; set; }

        // makes the table name singular instead of plural (overrides the default behavior)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Passenger>().ToTable("Passenger");
        }
    }
}