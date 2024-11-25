using Microsoft.EntityFrameworkCore;
using PcbDispatchService.Domain.Logic.States;
using PcbDispatchService.Domain.Models;

namespace PcbDispatchService.Dal;

public interface IPcbRepository
{
    Task<PrintedCircuitBoard?> GetPcbByName(string name);
    Task<PrintedCircuitBoard?> GetPcbById(int id);
    Task<List<PrintedCircuitBoard>> GetAllPcbs();
    Task AddNewBoard(PrintedCircuitBoard pcb);
    Task DeletePcbById(int id);
    Task RenameBoard(int id, string newName);
    Task RemoveComponentsFromBoard(int id);
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

    public async Task AddNewBoard(PrintedCircuitBoard pcb)
    {
        await _context.AddAsync(pcb);
        await _context.SaveChangesAsync();
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

    public async Task RenameBoard(int id, string newName)
    {
        var pcb = await _context.PrintedCircuitBoards.Where(i => i != null && i.Id == id).FirstOrDefaultAsync();
        if (pcb != null)
        {
            pcb.RenamePcb(newName);
            _context.PrintedCircuitBoards.Update(pcb);
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveComponentsFromBoard(int id)
    {
        var pcb = await _context.PrintedCircuitBoards.Where(i => i != null && i.Id == id).FirstOrDefaultAsync();
        if (pcb != null)
        {
            pcb.RemoveAllComponentsFromBoard();
            _context.PrintedCircuitBoards.Update(pcb);
            await _context.SaveChangesAsync();
        }
    }
}