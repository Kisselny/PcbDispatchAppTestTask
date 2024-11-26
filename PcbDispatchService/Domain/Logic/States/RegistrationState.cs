using PcbDispatchService.Domain.Models;
using PcbDispatchService.Services;

namespace PcbDispatchService.Domain.Logic.States;

public class RegistrationState : BusinessProcessStateBase
{
    private readonly IStateFactory _stateFactory;
    private readonly MyCustomLoggerService _myCustomLoggerService;
    private readonly BusinessRules _businessRules;

    public RegistrationState(IStateFactory stateFactory, MyCustomLoggerService myCustomLoggerService, BusinessRules businessRules)
    {
        _myCustomLoggerService = myCustomLoggerService;
        _businessRules = businessRules;
        _stateFactory = stateFactory;
    }

    public RegistrationState()
    {
    }

    public override void AdvanceToNextState(PrintedCircuitBoard printedCircuitBoard)
    {
        var result = _businessRules.CheckIfContinuationIsPossible(printedCircuitBoard);
        if(result == _businessRules.okMessage)
        {
            _myCustomLoggerService.LogThisSh_t("Регистрация пройдена, переход на этап добавления компонентов.");
            printedCircuitBoard.SetBusinessState(_stateFactory.CreateComponentInstallationState());
        }
        else
        {
            throw new BusinessException(result);
        }
    }

    public override BusinessProcessStatusEnum GetCurrentStatus()
    {
        return BusinessProcessStatusEnum.Registration;
    }
}