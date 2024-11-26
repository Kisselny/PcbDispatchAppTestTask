using PcbDispatchService.Domain.Logic.States;
using PcbDispatchService.Domain.Models;

namespace PcbDispatchService.Services;

public class PcbFactory
{
    public PcbFactory()
    {
    }

    public PrintedCircuitBoard CreateCircuitBoard(string name)
    {
        var brandNewBoard = new PrintedCircuitBoard(name);
        return brandNewBoard;
    }
}