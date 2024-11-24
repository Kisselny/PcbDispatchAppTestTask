namespace PcbDispatchService.Domain.Models;

public class PcbComponent
{
    public string Name { get; }
    public int Quantity { get; }

    public PcbComponent(string name, int quantity)
    {
        Name = name;
        Quantity = quantity;
    }
}