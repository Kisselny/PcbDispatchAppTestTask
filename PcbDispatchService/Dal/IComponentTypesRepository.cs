using PcbDispatchService.Domain.Models;

namespace PcbDispatchService.Dal;

public interface IComponentTypesRepository
{
    Task<ComponentType?> GetComponentTypeByName(string name);
    Task<List<ComponentType>> GetComponentTypesByNames(List<string> names);
    Task<List<ComponentType>> GetAllComponents();
    
    /// <summary>
    /// Увеличивает количество компонентов на складе.
    /// </summary>
    /// <param name="boardComponents">Коллекция компонентов и количество, на которое нужно увеличить.</param>
    /// <returns></returns>
    /// <remarks>Используется для возвращения компонентов на склад при очистке компонентов с платы.</remarks>
    Task IncreaseComponentSupplyByValue(List<BoardComponent> boardComponents);
    
    /// <summary>
    /// Уменьшает количество указанного компонента на складе.
    /// </summary>
    /// <param name="boardComponent">Компонент и количество, на которое уменьшить.</param>
    /// <returns></returns>
    Task DecreaseComponentSupplyByValue(string componentTypeName, int value);
}