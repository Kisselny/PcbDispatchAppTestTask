using Microsoft.EntityFrameworkCore;
using PcbDispatchService.Domain.Models;

namespace PcbDispatchService.Dal;

public class ComponentTypesRepository : IComponentTypesRepository
{
    private readonly ApplicationDbContext _context;

    public ComponentTypesRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ComponentType?> GetComponentTypeByName(string name)
    {
        var component = await _context.ComponentTypes.AsNoTracking().Where(i => i.Name == name).FirstOrDefaultAsync();
        if (component is null)
        {
            throw new ApplicationException($"Компонент типа {name} не найден в системе.");
        }

        return component;
    }

    
    public async Task<List<ComponentType>> GetComponentTypesByNames(List<string> names)
    {
        if (names.Count == 0)
        {
            return new List<ComponentType>();
        }

        var components = await _context.ComponentTypes
            .Where(i => names.Contains(i.Name))
            .ToListAsync();

        return components;
    }

    public async Task<List<ComponentType>> GetAllComponents()
    {
        return await _context.ComponentTypes.AsNoTracking().ToListAsync(); 
    }
}