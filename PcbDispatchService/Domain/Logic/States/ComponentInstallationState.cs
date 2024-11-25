using PcbDispatchService.Domain.Models;
using PcbDispatchService.Services;

namespace PcbDispatchService.Domain.Logic.States;

public class ComponentInstallationState : IBusinessProcessState
{
    private readonly IStateFactory _stateFactory;
    private readonly LoggerService _loggerService;
    private readonly BusinessRules _businessRules;

    public ComponentInstallationState(IStateFactory stateFactory, LoggerService loggerService, BusinessRules businessRules)
    {
        _stateFactory = stateFactory;
        _loggerService = loggerService;
        _businessRules = businessRules;
    }

    public void AdvanceToNextState(PrintedCircuitBoard printedCircuitBoard)
    {
        var result = _businessRules.CheckIfContinuationIsPossible(printedCircuitBoard);
        if(result == _businessRules.okMessage)
        {
            _loggerService.LogThisSh_t("Установка компонентов пройдена успешно, переход к контролю качества.");
            printedCircuitBoard.SetBusinessState(_stateFactory.CreateQualityControlState());
        }
        else
        {
            throw new BusinessException(result);
        }
    }

    public BusinessProcessStatusEnum GetCurrentStatus()
    {
        return BusinessProcessStatusEnum.ComponentInstallation;
    }

    public string GetCurrentStatusString()
    {
        return GetCurrentStatus().ToString();
    }
}