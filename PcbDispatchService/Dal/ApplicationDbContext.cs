using Microsoft.EntityFrameworkCore;
using PcbDispatchService.Domain.Models;

namespace PcbDispatchService.Dal;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<ComponentType?> ComponentTypes { get; set; }
    public DbSet<PrintedCircuitBoard> PrintedCircuitBoards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}