using PcbDispatchService.Domain.Models;

namespace PcbDispatchService.Dal;

public interface IComponentTypesRepository
{
    Task<ComponentType?> GetComponentTypeByName(string name);
    Task<List<ComponentType>> GetComponentTypesByNames(List<string> names);
    Task<List<ComponentType>> GetAllComponents();
}