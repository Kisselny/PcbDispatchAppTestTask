using PcbDispatchService.Domain.Models;
using PcbDispatchService.Services;

namespace PcbDispatchService.Domain.Logic.States;

public class ComponentInstallationState : BusinessProcessStateBase
{
    private readonly IStateFactory _stateFactory;
    private readonly MyCustomLoggerService _myCustomLoggerService;
    private readonly BusinessRules _businessRules;

    public ComponentInstallationState(IStateFactory stateFactory, MyCustomLoggerService myCustomLoggerService, BusinessRules businessRules)
    {
        _stateFactory = stateFactory;
        _myCustomLoggerService = myCustomLoggerService;
        _businessRules = businessRules;
    }

    public ComponentInstallationState()
    {
    }

    public override void AdvanceToNextState(PrintedCircuitBoard printedCircuitBoard)
    {
        var result = _businessRules.CheckIfContinuationIsPossible(printedCircuitBoard);
        if(result == _businessRules.okMessage)
        {
            _myCustomLoggerService.LogThisSh_t("Установка компонентов пройдена успешно, переход к контролю качества.");
            printedCircuitBoard.SetBusinessState(_stateFactory.CreateQualityControlState());
        }
        else
        {
            throw new BusinessException(result);
        }
    }

    public override BusinessProcessStatusEnum GetCurrentStatus()
    {
        return BusinessProcessStatusEnum.ComponentInstallation;
    }
}