using System.Runtime.Intrinsics.X86;
using PcbDispatchService.Domain.Logic.States;
using PcbDispatchService.Domain.Models;
using PcbDispatchService.Services;

namespace PcbDispatchService.Domain.Logic;

public class BusinessRules
{
    private readonly QualityControlService _qualityControlService;
    public readonly string okMessage = "Ok";
    private readonly string notOkMessageStart = "Невозможно продвинуться по бизнес-процессу: ";
    private readonly string notOkMessageRegistration = "Имя платы не должно быть пустым.";
    private readonly string notOkMessageComponents = "К плате необходимо добавить компоненты";
    private readonly string notOkMessageQuality = "Плата не прошла контроль качества. Необходимо отправить на ремонт.";
    private readonly string notOkMessageDefective = "Плата не прошла контроль качества, т.к. признана бракованной.";
    
    public BusinessRules(QualityControlService qualityControlService)
    {
        _qualityControlService = qualityControlService;
    }

    public string ContinuationIsPossible(Pcb pcb)
    {
        switch (pcb.GetBusinessState())
        {
            case BusinessProcessStatusEnum.Registration:
            {
                /*Вообще, конечно, в конструкторах платы уже завалидировано, что её имя не будет пустым,
                 но какая-то логика должна же быть))*/
                if (pcb.Name != string.Empty)
                {
                    return okMessage;
                }
                else
                    return notOkMessageStart + notOkMessageRegistration;
            }
            case BusinessProcessStatusEnum.ComponentInstallation:
            {
                return pcb.Components.Count != 0 ? okMessage : notOkMessageStart + notOkMessageComponents;
            }
            case BusinessProcessStatusEnum.QualityControl:
            {
                break;
            }
            case BusinessProcessStatusEnum.Repair:
            {
                if (pcb.QualityControlStatus == QualityControlStatus.QualityIsOk)
                    return okMessage;
                else if(pcb.QualityControlStatus == QualityControlStatus.Defective)
                {
                    return notOkMessageStart + notOkMessageDefective;
                }

                break;
            }
            case BusinessProcessStatusEnum.Packaging:
            {
                break;
            }
        }
        throw new InvalidOperationException();
    }
}