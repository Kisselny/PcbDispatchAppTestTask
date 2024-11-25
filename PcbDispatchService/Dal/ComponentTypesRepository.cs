using Microsoft.EntityFrameworkCore;
using PcbDispatchService.Domain.Models;

namespace PcbDispatchService.Dal;

public interface IComponentTypesRepository
{
    Task<ComponentType?> GetComponentTypeByName(string name);
    Task<List<ComponentType?>> GetComponentTypesByNames(List<string> names);
}

public class ComponentTypesRepository : IComponentTypesRepository
{
    private readonly ApplicationDbContext _context;

    public ComponentTypesRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ComponentType?> GetComponentTypeByName(string name)
    {
        return await _context.ComponentTypes.Where(i => i != null && i.Name == name).FirstOrDefaultAsync();
    }
    
    public async Task<List<ComponentType?>> GetComponentTypesByNames(List<string> names)
    {
        return await _context.ComponentTypes.Where(i => names.Any(s => s == i.Name)).ToListAsync();
    }
}