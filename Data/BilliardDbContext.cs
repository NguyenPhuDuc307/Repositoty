using Microsoft.EntityFrameworkCore;
using BilliardManagement.Data.Entities;

namespace BilliardManagement.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<PriceList>()
                .HasKey(c => new { c.TableId, c.BranchId });
    }

    public DbSet<Club> Clubs { get; set; } = null!;
    public DbSet<Branch> Branches { get; set; } = null!;
    public DbSet<Table> Tables { get; set; } = null!;
    public DbSet<PriceList> PriceLists { get; set; } = null!;
}