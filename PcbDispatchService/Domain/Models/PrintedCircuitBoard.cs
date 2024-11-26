using System.ComponentModel.DataAnnotations;
using PcbDispatchService.Domain.Logic;
using PcbDispatchService.Domain.Logic.States;

namespace PcbDispatchService.Domain.Models;

/// <summary>
/// Представляет сущность печатной платы.
/// </summary>
public class PrintedCircuitBoard
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
    [MaxLength(120)]
    public string Name { get; private set; }
    
    /// <summary>
    /// Компоненты платы.
    /// </summary>
    public List<BoardComponent> Components { get; }

    /// <summary>
    /// Статус бизнес-процесса.
    /// </summary>
    public BusinessProcessStatusEnum BusinessProcessStatus { get; private set; }
    
    /// <summary>
    /// Статус контроля качества.
    /// </summary>
    public QualityControlStatus QualityControlStatus { get; set; }
    #endregion


    #region .ctor

    /// <summary>
    /// Создает новый экземпляр печатной платы.
    /// </summary>
    /// <param name="name">Имя платы.</param>
    public PrintedCircuitBoard(string name)
    {
        Id = generateId();
        Name = validateNameNotEmpty(name);
        Components = new List<BoardComponent>();
        BusinessProcessStatus = BusinessProcessStatusEnum.Registration;
        QualityControlStatus = QualityControlStatus.NotSureYet;
    }

    public PrintedCircuitBoard() { }
    #endregion

    #region Public API

    public void SetBusinessEnum(BusinessProcessStatusEnum newBusinessProcessStatusEnum)
    {
        BusinessProcessStatus = newBusinessProcessStatusEnum;
    }
    
    /// <summary>
    /// Переименовывает печатную плату.
    /// </summary>
    /// <param name="newName"></param>
    /// <exception cref="BusinessException"></exception>
    public void RenamePcb(string newName)
    {
        if (BusinessProcessStatus is BusinessProcessStatusEnum.Registration)
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
        if (BusinessProcessStatus is BusinessProcessStatusEnum.ComponentInstallation)
        {
            var existing = Components.FirstOrDefault(i => i.ComponentType == newComponent.ComponentType); 
            if (existing is not null)
            {
                existing.Quantity = newComponent.Quantity;
            }
            else
            {
                Components.Add(newComponent);
            }
        }
        else
        {
            throw new BusinessException(
                "Невозможно добавить компонент: плата не находится на этапе добавления компонентов.");
        }
    }

    /// <summary>
    /// Удаляет все добавленные компоненты с текущей платы.
    /// </summary>
    /// <exception cref="BusinessException">Невозможно удалить компоненты.</exception>
    public void RemoveAllComponentsFromBoard()
    {
        if (BusinessProcessStatus is BusinessProcessStatusEnum.ComponentInstallation)
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