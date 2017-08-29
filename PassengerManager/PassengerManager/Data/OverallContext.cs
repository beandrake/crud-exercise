// adapted from: https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro

using PassengerManager.Models;
using Microsoft.EntityFrameworkCore;

namespace PassengerManager.Data
{
    /*
     * In the codeschool MVC ASP.NET Core lectures
     * (https://www.codeschool.com/courses/try-asp-net-core)
     * it seemed to be suggested that each Model n get its own nContext class.
     * However, in the tutorial used to create most of this program, a single
     * Context class was used.  I decided to follow the second approach,
     * primarily because it looked to be a more manageable method.
     * I intentionally gave it a very generic name, so as not to restrict
     * future development when other Models such as Cruises or Cruiseliner
     * are likely to be introduced.
     */
    public class OverallContext : DbContext
    {
        public OverallContext(DbContextOptions<OverallContext> options) : base(options)
        {
        }

        // any models we want to map to the database go here with this format
        public DbSet<Passenger> Passengers { get; set; }

        // makes the table name singular instead of plural (overrides the default behavior)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Passenger>().ToTable("Passenger");
        }
    }
}