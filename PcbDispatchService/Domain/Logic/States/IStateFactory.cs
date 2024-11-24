namespace PcbDispatchService.Domain.Logic.States;

public interface IStateFactory
{
    IBusinessProcessState CreateRegistrationState();
    IBusinessProcessState CreateComponentInstallationState();
    IBusinessProcessState CreateQualityControlState();
    IBusinessProcessState CreateRepairState();
    IBusinessProcessState CreatePackagingState();
}