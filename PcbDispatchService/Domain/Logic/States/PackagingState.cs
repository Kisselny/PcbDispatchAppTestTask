using PcbDispatchService.Domain.Models;
using PcbDispatchService.Services;

namespace PcbDispatchService.Domain.Logic.States;

public class PackagingState : IBusinessProcessState
{
    private readonly LoggerService _loggerService;
    private readonly BusinessRules _businessRules;
    private readonly QualityControlService _qualityControlService;

    public PackagingState(LoggerService loggerService, BusinessRules businessRules, QualityControlService qualityControlService)
    {
        _loggerService = loggerService;
        _businessRules = businessRules;
        _qualityControlService = qualityControlService;
    }

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