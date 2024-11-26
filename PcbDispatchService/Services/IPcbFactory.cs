using PcbDispatchService.Domain.Models;

namespace PcbDispatchService.Services;

/// <summary>
/// Фабрика создания платы. Когда-то тут было много функционала, но он уехал в сервис.
/// </summary>
public interface IPcbFactory
{
    /// <summary>
    /// Создает новую печатную плату в статусе "Регистрация".
    /// </summary>
    /// <param name="name">Имя новой платы.</param>
    /// <returns>Экземпляр печатной платы</returns>
    PrintedCircuitBoard CreateCircuitBoard(string name);
}