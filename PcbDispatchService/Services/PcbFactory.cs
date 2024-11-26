using PcbDispatchService.Domain.Logic.States;
using PcbDispatchService.Domain.Models;

namespace PcbDispatchService.Services;

public class PcbFactory
{
    private readonly IStateFactory _stateFactory;

    public PcbFactory(IStateFactory stateFactory)
    {
        _stateFactory = stateFactory;
    }

    public PrintedCircuitBoard CreateCircuitBoard(string name)
    {
        var brandNewBoard = new PrintedCircuitBoard(name, _stateFactory);
        return brandNewBoard;
    }
}