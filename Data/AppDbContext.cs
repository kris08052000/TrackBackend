using Microsoft.EntityFrameworkCore;
using TRACK_BACKEND.Models;

namespace TRACK_BACKEND.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Item> Items { get; set; }

    public DbSet<User> Users { get; set; }
}
