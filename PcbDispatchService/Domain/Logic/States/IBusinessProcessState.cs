using PcbDispatchService.Domain.Models;
using PcbDispatchService.Services;

namespace PcbDispatchService.Domain.Logic.States;

public interface IBusinessProcessState
{
    void AdvanceToNextState(Pcb pcb);
    BusinessProcessStatusEnum GetCurrentStatus();
}

public class RegistrationState : IBusinessProcessState
{
    //private BusinessRules _businessRules = new BusinessRules();
    public void AdvanceToNextState(Pcb pcb)
    {
        //if()
        pcb.SetBusinessState(new ComponentInstallationState());
    }

    public BusinessProcessStatusEnum GetCurrentStatus()
    {
        return BusinessProcessStatusEnum.Registration;
    }
}

public class ComponentInstallationState : IBusinessProcessState
{
    public void AdvanceToNextState(Pcb pcb)
    {
        pcb.SetBusinessState(new QualityControlState());
    }

    public BusinessProcessStatusEnum GetCurrentStatus()
    {
        return BusinessProcessStatusEnum.ComponentInstallation;
    }
}

public class QualityControlState : IBusinessProcessState
{
    private readonly QualityControlService _qualityControlService = new QualityControlService();
    private readonly LoggerService _loggerService = new LoggerService();

    public void AdvanceToNextState(Pcb pcb)
    {
        if (_qualityControlService.QualityCheck(pcb) == QualityControlStatus.QualityIsOk)
        {
            _loggerService.LogThisSh_t("Качество в порядке, переход к шагу \"Упаковка\"");
            pcb.SetBusinessState(new PackagingState());
        }
        else
        {
            _loggerService.LogThisSh_t("Качество не в порядке, переход к шагу \"Ремонт\"");
            pcb.SetBusinessState(new RepairState());
        }
    }

    public BusinessProcessStatusEnum GetCurrentStatus()
    {
        return BusinessProcessStatusEnum.QualityControl;
    }
}

public class RepairState : IBusinessProcessState
{
    private readonly QualityControlService _qualityControlService = new QualityControlService();
    private readonly LoggerService _loggerService = new LoggerService();
    public void AdvanceToNextState(Pcb pcb)
    {
        pcb.QualityControlStatus = _qualityControlService.TryRepair(pcb);
        switch (pcb.QualityControlStatus)
        {
            case QualityControlStatus.QualityIsOk:
                _loggerService.LogThisSh_t("Ремонт произведён, возвращение к шагу \"Контроль качества\"");
                pcb.SetBusinessState(new QualityControlState());
                break;
            default:
                _loggerService.LogThisSh_t("Ремонт не удалось произвести, плата признана браком.");
                pcb.QualityControlStatus = QualityControlStatus.Defective;
                break;
        }
    }

    public BusinessProcessStatusEnum GetCurrentStatus()
    {
        return BusinessProcessStatusEnum.Repair;
    }
}

public class PackagingState : IBusinessProcessState
{
    private readonly LoggerService _loggerService = new LoggerService();
    private bool PackagedAndReleased = false;
    
    //TODO можно еще какую-то логику внедрить.
    public void AdvanceToNextState(Pcb pcb)
    {
        if (PackagedAndReleased)
        {
            _loggerService.LogThisSh_t("Данная плата уже была отгружена. Повторная отгрузка невозможна.");
        }
        else
        {
            _loggerService.LogThisSh_t("Плата успешно произведена и отгружена.");
            PackagedAndReleased = true;
        }
    }

    public BusinessProcessStatusEnum GetCurrentStatus()
    {
        return BusinessProcessStatusEnum.Packaging;
    }
}