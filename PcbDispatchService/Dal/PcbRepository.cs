using Microsoft.EntityFrameworkCore;
using PcbDispatchService.Domain.Logic.States;
using PcbDispatchService.Domain.Models;

namespace PcbDispatchService.Dal;

public interface IPcbRepository
{
    Task<PrintedCircuitBoard?> GetPcbByName(string name);
    Task<PrintedCircuitBoard?> GetPcbById(int id);
    Task<List<PrintedCircuitBoard>> GetAllPcbs();
    Task AddPcb(PrintedCircuitBoard pcb);
    Task DeletePcbByName(string name);
    Task DeletePcbById(int id);
}

public class PcbRepository : IPcbRepository
{
    private readonly ApplicationDbContext _context;

    public PcbRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PrintedCircuitBoard?> GetPcbByName(string name)
    {
        return await _context.PrintedCircuitBoards.Where(i => i != null && i.Name == name).FirstOrDefaultAsync();
    }
    
    public async Task<PrintedCircuitBoard?> GetPcbById(int id)
    {
        return await _context.PrintedCircuitBoards.Where(i => i != null && i.Id == id).FirstOrDefaultAsync();
    }
    
    public async Task<List<PrintedCircuitBoard>> GetAllPcbs()
    {
        return await _context.PrintedCircuitBoards.ToListAsync();
    }

    public async Task AddPcb(PrintedCircuitBoard pcb)
    {
        await _context.AddAsync(pcb);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePcbByName(string name)
    {
        var pcb = await _context.PrintedCircuitBoards.Where(i => i != null && i.Name == name).FirstOrDefaultAsync();
        if (pcb != null)
        {
            _context.PrintedCircuitBoards.Remove(pcb);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task DeletePcbById(int id)
    {
        var pcb = await _context.PrintedCircuitBoards.Where(i => i != null && i.Id == id).FirstOrDefaultAsync();
        if (pcb != null)
        {
            _context.PrintedCircuitBoards.Remove(pcb);
            await _context.SaveChangesAsync();
        }
    }
}