using System.ComponentModel.DataAnnotations;

namespace PcbDispatchService.Domain.Models;

/// <summary>
/// Представляет компонент на плате.
/// </summary>
public class BoardComponent
{
    /// <summary>
    /// Идентификатор компонента, помещенного на плату.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Тип компонента.
    /// </summary>
    [MaxLength(120)]
    public string ComponentType { get; private set; }
    
    /// <summary>
    /// Количество компонентов, добавляемых к плате.
    /// </summary>
    public int Quantity { get; set; }


    /// <summary>
    /// Создает экземпляр(-ы) компонента для добавления на плату.
    /// </summary>
    /// <param name="type">Тип компонента.</param>
    /// <param name="quantity">Количество.</param>
    public BoardComponent(ComponentType type, int quantity)
    {
        ComponentType = type.Name;
        Quantity = quantity;
    }
    
    public BoardComponent() { }
}