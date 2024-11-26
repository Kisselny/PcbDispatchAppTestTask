namespace PcbDispatchService.Domain.Models;

/// <summary>
/// Представляет уникальный тип компонента.
/// </summary>
public class ComponentType
{
    /// <summary>
    /// Название типа компонента.
    /// </summary>
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
    
    public static ComponentType Create(string typeName, int availableSupply)
    {
        return new ComponentType(typeName, availableSupply);
    }
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