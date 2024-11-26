using PcbDispatchService.Domain.Models;

namespace PcbDispatchService.Dal;

/// <summary>
/// Репозиторий компонентов для плат.
/// </summary>
public interface IComponentTypesRepository
{
    /// <summary>
    /// Найти компонент по имени.
    /// </summary>
    /// <param name="name">Название компонента.</param>
    /// <returns>Компонент.</returns>
    Task<ComponentType?> GetComponentTypeByName(string name);
    /// <summary>
    /// Получить все компоненты на складе.
    /// </summary>
    /// <returns></returns>
    Task<List<ComponentType>> GetAllComponents();
    
    /// <summary>
    /// Увеличивает количество компонентов на складе.
    /// </summary>
    /// <param name="boardComponents">Коллекция компонентов и количество, на которое нужно увеличить.</param>
    /// <returns></returns>
    /// <remarks>Используется для возвращения компонентов на склад при очистке компонентов с платы.</remarks>
    Task IncreaseComponentSupplyByValue(List<BoardComponent> boardComponents);
    

    /// <summary>
    /// Уменьшить количество компонента ан складе.
    /// </summary>
    /// <param name="componentTypeName">Название типа компонента.</param>
    /// <param name="value">Количество</param>
    /// <returns></returns>
    Task DecreaseComponentSupplyByValue(string componentTypeName, int value);
}