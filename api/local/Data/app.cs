using Microsoft.EntityFrameworkCore;

public class AppDbContext: DbContext
{
    // Connection
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    // Relations
    public DbSet<Item> Items { get; set; }
}