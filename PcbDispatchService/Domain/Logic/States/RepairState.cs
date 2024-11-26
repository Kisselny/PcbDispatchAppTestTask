using PcbDispatchService.Domain.Models;
using PcbDispatchService.Services;

namespace PcbDispatchService.Domain.Logic.States;

public class RepairState : BusinessProcessStateBase
{
    private readonly IStateFactory _stateFactory;
    private readonly MyCustomLoggerService _myCustomLoggerService;
    private readonly BusinessRules _businessRules;

    public RepairState(IStateFactory stateFactory, MyCustomLoggerService myCustomLoggerService, BusinessRules businessRules)
    {
        _stateFactory = stateFactory;
        _myCustomLoggerService = myCustomLoggerService;
        _businessRules = businessRules;
    }

    public RepairState()
    {
    }

    public override void AdvanceToNextState(PrintedCircuitBoard printedCircuitBoard)
    {
        var result = _businessRules.CheckIfContinuationIsPossible(printedCircuitBoard);

        if (result == _businessRules.okMessage)
        {
            _myCustomLoggerService.LogThisSh_t("Ремонт произведён, возвращение к шагу \"Контроль качества\"");
            printedCircuitBoard.SetBusinessState(_stateFactory.CreateQualityControlState());
        }
        else
        {
            throw new BusinessException(result);
        }
    }

    public override BusinessProcessStatusEnum GetCurrentStatus()
    {
        return BusinessProcessStatusEnum.Repair;
    }
}