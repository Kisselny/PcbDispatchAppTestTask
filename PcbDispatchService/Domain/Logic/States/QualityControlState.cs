using PcbDispatchService.Domain.Models;
using PcbDispatchService.Services;

namespace PcbDispatchService.Domain.Logic.States;

public class QualityControlState : BusinessProcessStateBase
{
    private readonly IStateFactory _stateFactory;
    private readonly MyCustomLoggerService _myCustomLoggerService;
    private readonly BusinessRules _businessRules;

    public QualityControlState(IStateFactory stateFactory, MyCustomLoggerService myCustomLoggerService, BusinessRules businessRules)
    {
        _stateFactory = stateFactory;
        _myCustomLoggerService = myCustomLoggerService;
        _businessRules = businessRules;
    }

    public QualityControlState()
    {
    }

    public override void AdvanceToNextState(PrintedCircuitBoard printedCircuitBoard)
    {
        var result = _businessRules.CheckIfContinuationIsPossible(printedCircuitBoard);
        if (result == _businessRules.okMessage)
        {
            _myCustomLoggerService.LogThisSh_t("Качество в порядке, переход к шагу \"Упаковка\"");
            printedCircuitBoard.SetBusinessState(_stateFactory.CreatePackagingState());
        }
        else
        {
            _myCustomLoggerService.LogThisSh_t(result);
            printedCircuitBoard.SetBusinessState(_stateFactory.CreateRepairState());
        }
    }

    public override BusinessProcessStatusEnum GetCurrentStatus()
    {
        return BusinessProcessStatusEnum.QualityControl;
    }
}