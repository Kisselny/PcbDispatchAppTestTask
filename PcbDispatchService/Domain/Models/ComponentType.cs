using System.ComponentModel.DataAnnotations;

namespace PcbDispatchService.Domain.Models;

/// <summary>
/// Представляет уникальный тип компонента.
/// </summary>
public class ComponentType
{
    /// <summary>
    /// Название типа компонента.
    /// </summary>
    [MaxLength(120)]
    public string Name { get; private set; }
    
    /// <summary>
    /// Имеющийся на складе запас.
    /// </summary>
    public int AvailableSupply { get; private set; }
    
    private ComponentType(string typeName, int availableSupply)
    {
        Name = typeName;
        AvailableSupply = availableSupply;
    }
    
    /// <summary>
    /// Создает экземпляр типа компонента из базы.
    /// </summary>
    /// <param name="typeName">Имя типа.</param>
    /// <param name="availableSupply">Имеющийся запа.</param>
    /// <returns>Тип компонента.</returns>
    public static ComponentType Create(string typeName, int availableSupply)
    {
        return new ComponentType(typeName, availableSupply);
    }
    /// <summary>
    /// Увеличить запас на складе.
    /// </summary>
    /// <param name="value">Количество</param>
    /// <remarks>При установке компонента на плату.</remarks>
    public void IncreaseSupply(int value)
    {
        if (value > 0)
        {
            AvailableSupply += value;
        }
        else
        {
            AvailableSupply -= value;
        }
    }
    
    /// <summary>
    /// Уменьшить запас на складе.
    /// </summary>
    /// <param name="value">Количество.</param>
    /// <remarks>При установке компонента на плату.</remarks>
    public void DecreaseSupply(int value)
    {
        if (value > 0)
        {
            AvailableSupply -= value;
        }
        else
        {
            AvailableSupply += value;
        }
    }
    
    public ComponentType() { }

}