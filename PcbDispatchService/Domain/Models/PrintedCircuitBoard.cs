using PcbDispatchService.Domain.Logic;
using PcbDispatchService.Domain.Logic.States;

namespace PcbDispatchService.Domain.Models;

/// <summary>
/// Представляет сущность печатной платы.
/// </summary>
public class PrintedCircuitBoard
{
    private readonly IStateFactory _stateFactory;

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
    public List<BoardComponent> Components { get; }

    /// <summary>
    /// Статус бизнес-процесса.
    /// </summary>

    public BusinessProcessStateBase BusinessProcessStateBase { get; private set; }
    
    /// <summary>
    /// Статус контроля качества.
    /// </summary>
    public QualityControlStatus QualityControlStatus { get; set; }
    #endregion


    #region .ctor
    /// <summary>
    /// Инициализирует экзампляр класса <see cref="PrintedCircuitBoard"/>
    /// </summary>
    /// <param name="name">Название печатной платы.</param>
    /// <param name="stateFactory">Фабрика состояний.</param>
    public PrintedCircuitBoard(string name, IStateFactory stateFactory)
    {
        Id = generateId();
        Name = validateNameNotEmpty(name);
        Components = new List<BoardComponent>();
        _stateFactory = stateFactory;
        BusinessProcessStateBase = _stateFactory.CreateRegistrationState();
        QualityControlStatus = QualityControlStatus.NotSureYet;
    }

    public PrintedCircuitBoard() { }
    #endregion

    #region Public API
    /// <summary>
    /// Возвращает текущее состояние бизнес-процесса.
    /// </summary>
    /// <returns>Объект, описывающий состояние безнес-процесса.</returns>
    public BusinessProcessStatusEnum GetBusinessState()
    {
        return BusinessProcessStateBase.GetCurrentStatus();
    }

    /// <summary>
    /// Назначает новое состояние безнес-процесса.
    /// </summary>
    /// <param name="businessProcessStateBase">Объект, описывающий состояние безнес-процесса.</param>
    public void SetBusinessState(BusinessProcessStateBase businessProcessStateBase)
    {
        BusinessProcessStateBase = businessProcessStateBase;
    }
    
    /// <summary>
    /// Переименовывает печатную плату.
    /// </summary>
    /// <param name="newName"></param>
    /// <exception cref="BusinessException"></exception>
    public void RenamePcb(string newName)
    {
        if (BusinessProcessStateBase.GetCurrentStatus() is BusinessProcessStatusEnum.Registration)
        {
            Name = validateNameNotEmpty(newName);
        }
        else
        {
            throw new BusinessException(
                "Невозможно переименовать плату: переименование возможно только на этапе регистрации.");
        }
    }
    
    /// <summary>
    /// Добавляет новый компонент к печатной плате.
    /// </summary>
    /// <param name="newComponent">Новый компонент.</param>
    /// <exception cref="BusinessException">Невозможно добавить компонент.</exception>
    public void AddComponentToPcb(BoardComponent newComponent)
    {
        if (BusinessProcessStateBase.GetCurrentStatus() is BusinessProcessStatusEnum.ComponentInstallation)
        {
            Components.Add(newComponent);
        }
        else
        {
            throw new BusinessException(
                "Невозможно добавить компонент: плата не находится на этапе добавления компонентов.");
        }
    }

    /// <summary>
    /// Добавляет новую коллекцию компонентов к печатной плате.
    /// </summary>
    /// <param name="newComponents">Новые компоненты</param>
    /// <exception cref="BusinessException">Невозможно добавить компоненты.</exception>
    public void AddComponentsToPcb(IEnumerable<BoardComponent> newComponents)
    {
        if (BusinessProcessStateBase.GetCurrentStatus() is BusinessProcessStatusEnum.ComponentInstallation)
        {
            Components.AddRange(newComponents);
        }
        else
        {
            throw new BusinessException(
                "Невозможно добавить компоненты: плата не находится на этапе добавления компонентов.");
        }
    }

    /// <summary>
    /// Удаляет все добавленные компоненты с текущей платы.
    /// </summary>
    /// <exception cref="BusinessException">Невозможно удалить компоненты.</exception>
    public void RemoveAllComponentsFromBoard()
    {
        if (BusinessProcessStateBase.GetCurrentStatus() is BusinessProcessStatusEnum.ComponentInstallation)
        {
            Components.Clear();
        }
        else
        {
            throw new BusinessException(
                "Невозможно удалить компоненты: плата не находится на этапе добавления компонентов.");
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