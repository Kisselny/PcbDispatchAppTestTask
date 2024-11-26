namespace PcbDispatchService.Domain.Models;

/// <summary>
/// Представляет компонент на плате.
/// </summary>
public class BoardComponent
{
    public int Id { get; set; }

    /// <summary>
    /// Тип компонента.
    /// </summary>
    public ComponentType ComponentType { get; private set; }
    
    /// <summary>
    /// Количество компонентов, добавляемых к плате.
    /// </summary>
    public int Quantity { get; private set; }


    /// <summary>
    /// Создает экземпляр(-ы) компонента для добавления на плату.
    /// </summary>
    /// <param name="type">Тип компонента.</param>
    /// <param name="quantity">Количество.</param>
    public BoardComponent(ComponentType type, int quantity)
    {
        ComponentType = type;
        Quantity = quantity;
    }
    
    public BoardComponent() { }
}