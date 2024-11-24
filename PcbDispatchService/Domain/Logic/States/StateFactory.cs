using PcbDispatchService.Services;

namespace PcbDispatchService.Domain.Logic.States;

public class StateFactory : IStateFactory
{
    private readonly LoggerService _loggerService;
    private readonly BusinessRules _businessRules;
    private readonly QualityControlService _qualityControlService;

    public StateFactory(LoggerService loggerService, BusinessRules businessRules, QualityControlService qualityControlService)
    {
        _loggerService = loggerService;
        _businessRules = businessRules;
        _qualityControlService = qualityControlService;
    }

    public IBusinessProcessState CreateRegistrationState()
    {
        return new RegistrationState(this, _loggerService, _businessRules, _qualityControlService);
    }

    public IBusinessProcessState CreateComponentInstallationState()
    {
        return new ComponentInstallationState(this, _loggerService, _businessRules);
    }

    public IBusinessProcessState CreateQualityControlState()
    {
        return new QualityControlState(this, _loggerService, _businessRules);
    }

    public IBusinessProcessState CreateRepairState()
    {
        return new RepairState(this, _loggerService, _businessRules);
    }

    public IBusinessProcessState CreatePackagingState()
    {
        return new PackagingState(_loggerService);
    }
}