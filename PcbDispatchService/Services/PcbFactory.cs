using PcbDispatchService.Domain.Logic.States;
using PcbDispatchService.Domain.Models;

namespace PcbDispatchService.Services;

public class PcbFactory
{
    private readonly IStateFactory _stateFactory;
    private readonly LoggerService _loggerService;

    public PcbFactory(IStateFactory stateFactory, LoggerService loggerService)
    {
        _stateFactory = stateFactory;
        _loggerService = loggerService;
    }

    public PrintedCircuitBoard CreateCircuitBoard(string name)
    {
        var brandNewBoard = new PrintedCircuitBoard(name, _stateFactory);
        return brandNewBoard;
    }

    public void AddComponentToBoard(PrintedCircuitBoard pcb, ComponentType componentType, int quantity)
    {
        if (componentType.AvailableSupply >= quantity)
        {
            var addingComponent = new BoardComponent(componentType, quantity);
            pcb.AddComponentToPcb(addingComponent);
        }
        else
        {
            throw new ApplicationException(
                "Невозможно добавить компонент к плате, т.к. недостаточно компонентов указанного типа на складе.");
        }

        _loggerService.LogThisSh_t($"К плате {pcb.Name} добавлен новый компонент");
        throw new ApplicationException("Невозможно добавить компонент к плате, т.к. тип компонента не найден.");
    }

    public void AddComponentsToBoard(PrintedCircuitBoard pcb, List<BoardComponent> boardComponents, List<ComponentType?> componentTypes)
    {
        var names = boardComponents.Select(i => i.ComponentType.Name).ToList();
        
        foreach (var boardComponent in boardComponents)
        {
            var singleAvailableComponent = componentTypes.FirstOrDefault(i => i.Name == boardComponent.ComponentType.Name);
            if (singleAvailableComponent != null)
            {
                if (boardComponent.Quantity <= componentTypes
                        .First(i => i.Name == boardComponent.ComponentType.Name).AvailableSupply)
                {
                    var addingComponent = new BoardComponent(boardComponent.ComponentType, boardComponent.Quantity);
                    pcb.AddComponentToPcb(addingComponent);
                }
                else
                {
                    throw new ApplicationException($"Невозможно добавить компонент типа {boardComponent.ComponentType.Name} к плате, т.к. недостаточно компонентов указанного типа на складе.");
                }
            }
            else
            {
                throw new ApplicationException($"Компонент типа {boardComponent.ComponentType.Name} не найден в базе.");
            }
        }
        _loggerService.LogThisSh_t($"К плате {pcb.Name} добавлены новые компоненты");
    }
}