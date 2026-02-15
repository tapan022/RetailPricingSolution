using Microsoft.EntityFrameworkCore;



public class RetailDbContext : DbContext
{
    public RetailDbContext(DbContextOptions<RetailDbContext> options)
        : base(options)
    {
    }

    public DbSet<PricingRecord> PricingRecords { get; set; }
}