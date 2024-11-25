namespace PcbDispatchService.Domain.Models;

/// <summary>
/// Представляет компонент на плате.
/// </summary>
public class BoardComponent
{
    /// <summary>
    /// Тип компонента.
    /// </summary>
    public ComponentType ComponentType { get; }
    
    /// <summary>
    /// Количество компонентов, добавляемых к плате.
    /// </summary>
    public int Quantity { get; }

    /// <summary>
    /// Создает экземпляр(-ы) компонента для добавления на плату.
    /// </summary>
    /// <param name="typeName">Тип компонента.</param>
    /// <param name="quantity">Количество.</param>
    public BoardComponent(ComponentType type, int quantity)
    {
        ComponentType = type;
        Quantity = quantity;
    }
}