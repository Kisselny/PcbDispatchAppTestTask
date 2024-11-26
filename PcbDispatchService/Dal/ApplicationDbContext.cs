using Microsoft.EntityFrameworkCore;
using PcbDispatchService.Domain.Logic.States;
using PcbDispatchService.Domain.Models;

namespace PcbDispatchService.Dal;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<ComponentType> ComponentTypes { get; set; }
    public DbSet<PrintedCircuitBoard> PrintedCircuitBoards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PrintedCircuitBoard>().HasKey(pcb => pcb.Id);
        modelBuilder.Entity<BoardComponent>().HasKey(bc => bc.Id);
        modelBuilder.Entity<ComponentType>().HasKey(ct => ct.Name);

        modelBuilder.Entity<BusinessProcessStateBase>()
            .HasDiscriminator<string>("BusinessProcessType")
            .HasValue<RegistrationState>("Registration")
            .HasValue<ComponentInstallationState>("ComponentInstallation")
            .HasValue<QualityControlState>("QualityControlState")
            .HasValue<RepairState>("RepairState")
            .HasValue<PackagingState>("PackagingState");

        modelBuilder.Entity<ComponentType>().HasData(
            ComponentType.Create("A0-B0", 28),
            ComponentType.Create("A0-B1", 66),
            ComponentType.Create("A1-B0", 47),
            ComponentType.Create("A1-B1", 12)
        );
    }
}