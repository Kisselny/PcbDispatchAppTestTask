namespace PcbDispatchService.Domain.Models;

/// <summary>
/// Представляет уникальный тип компонента.
/// </summary>
public class ComponentType
{
    private readonly string _typeName;
    private readonly string _availableSupply;

    /// <summary>
    /// Представляет уникальный тип компонента.
    /// </summary>
    /// <param name="TypeName">Название типа.</param>
    public ComponentType(string TypeName)
    {
        _typeName = TypeName;
    }

    /*public bool FindComponent(string TypeName)
    {
        
    }

    public bool IsEnoughAvailable(int quantity)
    {
        
    }*/
}