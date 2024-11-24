using PcbDispatchService.Domain.Models;
using PcbDispatchService.Services;

namespace PcbDispatchService.Domain.Logic.States;

public class RepairState : IBusinessProcessState
{
    private readonly IStateFactory _stateFactory;
    private readonly LoggerService _loggerService;
    private readonly BusinessRules _businessRules;

    public RepairState(IStateFactory stateFactory, LoggerService loggerService, BusinessRules businessRules)
    {
        _stateFactory = stateFactory;
        _loggerService = loggerService;
        _businessRules = businessRules;
    }

    public void AdvanceToNextState(Pcb pcb)
    {
        var result = _businessRules.CheckIfContinuationIsPossible(pcb);

        if (result == _businessRules.okMessage)
        {
            _loggerService.LogThisSh_t("Ремонт произведён, возвращение к шагу \"Контроль качества\"");
            pcb.SetBusinessState(_stateFactory.CreateQualityControlState());
        }
        else
        {
            throw new BusinessException(result);
        }
    }

    public BusinessProcessStatusEnum GetCurrentStatus()
    {
        return BusinessProcessStatusEnum.Repair;
    }
}