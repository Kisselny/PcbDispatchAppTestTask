using PcbDispatchService.Domain.Logic.States;
using PcbDispatchService.Domain.Models;

namespace PcbDispatchService.Services;

/// <summary>
/// Фабрика создания платы. Когда-то тут было много функционала, но он уехал в сервис.
/// </summary>
public class PcbFactory : IPcbFactory
{
    public PcbFactory()
    {
    }

    /// <summary>
    /// Создает новую печатную плату в статусе "Регистрация".
    /// </summary>
    /// <param name="name">Имя новой платы.</param>
    /// <returns>Экземпляр печатной платы</returns>
    public PrintedCircuitBoard CreateCircuitBoard(string name)
    {
        if (name is null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        var brandNewBoard = new PrintedCircuitBoard(name);
        return brandNewBoard;
    }
}