using PcbDispatchService.Domain.Logic;
using PcbDispatchService.Domain.Logic.States;

namespace PcbDispatchService.Domain.Models;

public class Pcb
{
    #region Properties
    /// <summary>
    /// Уникальный идентификатор платы.
    /// <remarks>Здесь использем int исключительно в демонстрационных целях, чтобы не заморачиваться с Guid-ами.</remarks>
    /// </summary>
    public int Id;
    
    /// <summary>
    /// Название платы.
    /// </summary>
    public string Name { get; private set; }
    
    /// <summary>
    /// Компоненты платы.
    /// </summary>
    public List<PcbComponent> Components { get; }

    /// <summary>
    /// Статус бизнес-процесса.
    /// </summary>

    private IBusinessProcessState BusinessProcessState;
    
    /// <summary>
    /// Статус контроля качества.
    /// </summary>
    public QualityControlStatus QualityControlStatus { get; set; }
    #endregion


    #region .ctor
    public Pcb(string name)
    {
        Id = generateId();
        Name = validateNameNotEmpty(name);
        Components = new List<PcbComponent>();
        BusinessProcessState = new RegistrationState();
        QualityControlStatus = QualityControlStatus.NotSureYet;
    }
    #endregion

    #region Public API
    public BusinessProcessStatusEnum GetBusinessState()
    {
        return BusinessProcessState.GetCurrentStatus();
    }

    public void SetBusinessState(IBusinessProcessState businessProcessState)
    {
        BusinessProcessState = businessProcessState;
    }
    
    public void RenamePcb(string newName)
    {
        if (BusinessProcessState.GetCurrentStatus() is BusinessProcessStatusEnum.Registration)
        {
            Name = validateNameNotEmpty(newName);
        }
        else
        {
            throw new BusinessException(
                "Невозможно переименовать плату: переименование возможно только на этапе регистрации.");
        }
    }
    
    public void AddComponentToPcb(PcbComponent newComponent)
    {
        if (BusinessProcessState.GetCurrentStatus() is BusinessProcessStatusEnum.ComponentInstallation)
        {
            Components.Add(newComponent);
        }
        else
        {
            throw new BusinessException(
                "Невозможно добавить компонент: плата не находится на этапе добавления компонентов.");
        }
    }

    public void AddComponentsToPcb(IEnumerable<PcbComponent> newComponents)
    {
        if (BusinessProcessState.GetCurrentStatus() is BusinessProcessStatusEnum.ComponentInstallation)
        {
            Components.AddRange(newComponents);
        }
        else
        {
            throw new BusinessException(
                "Невозможно добавить компоненты: плата не находится на этапе добавления компонентов.");
        }
    }
    #endregion

    #region Private Methods
    private int generateId()
    {
        //Случайного шестизначного id должно хватить для демо-приложения, опять же, чтоб не морочиться с Guid.
        var r = new Random();
        return r.Next(100000, 999999);
    }

    private string validateNameNotEmpty(string name)
    {
        if (name == string.Empty)
        {
            throw new BusinessException("Название платы не должно быть пустым.");
        }
        else
        {
            return name;
        }
    }
    #endregion
}