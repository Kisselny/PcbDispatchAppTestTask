using Microsoft.EntityFrameworkCore;
using PcbDispatchService.Domain.Logic.States;
using PcbDispatchService.Domain.Models;

namespace PcbDispatchService.Dal;

/// <inheritdoc />
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
    
    /// <summary>
    /// Типы компонентов.
    /// </summary>
    public DbSet<ComponentType> ComponentTypes { get; set; }
    
    /// <summary>
    /// Печатные платы.
    /// </summary>
    public DbSet<PrintedCircuitBoard> PrintedCircuitBoards { get; set; }
    
    /// <summary>
    /// Компоненты, установленные на плату.
    /// </summary>
    public DbSet<BoardComponent> BoardComponents { get; set; }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        

        
        modelBuilder.Entity<PrintedCircuitBoard>().HasKey(pcb => pcb.Id);
        
        
        modelBuilder.Entity<BoardComponent>().HasKey(bc => bc.Id);
        
        
        modelBuilder.Entity<ComponentType>().HasKey(ct => ct.Name);

        modelBuilder.Entity<ComponentType>().HasData(
            ComponentType.Create("A0-B0", 28),
            ComponentType.Create("A0-B1", 66),
            ComponentType.Create("A1-B0", 47),
            ComponentType.Create("A1-B1", 12)
        );
    }
}