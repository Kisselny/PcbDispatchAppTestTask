using PcbDispatchService.Domain.Logic.States;
using PcbDispatchService.Domain.Models;

namespace PcbDispatchService.Services;

public class PcbFactory
{
    private readonly IStateFactory _stateFactory;
    private readonly MyCustomLoggerService _myCustomLoggerService;

    public PcbFactory(IStateFactory stateFactory, MyCustomLoggerService myCustomLoggerService)
    {
        _stateFactory = stateFactory;
        _myCustomLoggerService = myCustomLoggerService;
    }

    public PrintedCircuitBoard CreateCircuitBoard(string name)
    {
        var brandNewBoard = new PrintedCircuitBoard(name, _stateFactory);
        return brandNewBoard;
    }
}