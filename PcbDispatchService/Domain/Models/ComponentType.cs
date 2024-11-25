namespace PcbDispatchService.Domain.Models;

/// <summary>
/// Представляет уникальный тип компонента.
/// </summary>
public class ComponentType
{
    private readonly string _typeName;
    private readonly int _availableSupply;

    public string Name => _typeName;
    public int AvailableSupply => _availableSupply;

    /// <summary>
    /// Представляет уникальный тип компонента.
    /// </summary>
    /// <param name="typeName">Название типа.</param>
    private ComponentType(string typeName, int availableSupply)
    {
        _typeName = typeName;
        _availableSupply = availableSupply;
    }
}