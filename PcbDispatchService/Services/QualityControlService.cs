using PcbDispatchService.Domain.Logic;
using PcbDispatchService.Domain.Models;

namespace PcbDispatchService.Services;

public class QualityControlService
{
    /// <summary>
    /// Выполняет проверку качества.
    /// </summary>
    /// <param name="printedCircuitBoard">Экземпляр платы.</param>
    /// <returns>Статус контроля качества.</returns>
    public QualityControlStatus QualityCheck(PrintedCircuitBoard printedCircuitBoard)
    {
        if (великийКорейскийРандом() > 30)
        {
            return QualityControlStatus.QualityIsOk;
        }
        else
        {
            return QualityControlStatus.QualityIsBad;
        }
    }
    
    /// <summary>
    /// Выполняет попытку ремонта платы.
    /// </summary>
    /// <param name="printedCircuitBoard">Экземпляр платы.</param>
    /// <returns>Новый статус контроля качества.</returns>
    /// <exception cref="BusinessException"></exception>
    public QualityControlStatus TryRepair(PrintedCircuitBoard printedCircuitBoard)
    {
        switch (printedCircuitBoard.QualityControlStatus)
        {
            case QualityControlStatus.QualityIsBad:
            {
                return QualityCheck(printedCircuitBoard);
            }
            case QualityControlStatus.Defective:
                throw new BusinessException("Невозможно починить плату, т.к. она уже признана бракованной.");
            case QualityControlStatus.NotSureYet:
                throw new BusinessException("Невозможно починить плату, т.к. она еще не проходила контроль качества и не признана поврежденной.");
            default:
                throw new BusinessException("Невозможно починить плату, т.к. она не нуждается в ремонте.");
        }
    }
    
    private int великийКорейскийРандом()
    {
        //а как еще определять качество?..
        var koreanRandom = new Random();
        return koreanRandom.Next(100);
    }
}