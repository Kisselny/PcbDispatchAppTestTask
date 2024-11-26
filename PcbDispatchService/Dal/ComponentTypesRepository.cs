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


    /// <inheritdoc />
    public async Task IncreaseComponentSupplyByValue(List<BoardComponent> boardComponents)
    {
        var storageComponents = await _context.ComponentTypes
            .Where(i => boardComponents.Any(bc => bc.ComponentType.Name == i.Name)).ToListAsync();
        if (storageComponents is null)
        {
            throw new ApplicationException($"storageComponents is null");
        }

        foreach (var storageComponent in storageComponents)
        {
            int value = boardComponents.First(i => i.ComponentType.Name == storageComponent.Name).Quantity;
            storageComponent.IncreaseSupply(value);
        }
        _context.ComponentTypes.UpdateRange(storageComponents);
        await _context.SaveChangesAsync();
    }
    
    public async Task DecreaseComponentSupplyByValue(string componentTypeName, int value)
    {
        var component = await _context.ComponentTypes.Where(i => i.Name == componentTypeName).FirstOrDefaultAsync();
        if (component is null)
        {
            throw new ApplicationException($"Компонент типа {componentTypeName} не найден в системе.");
        }
        component.DecreaseSupply(value);
        _context.ComponentTypes.Update(component);
        await _context.SaveChangesAsync();
    }
}