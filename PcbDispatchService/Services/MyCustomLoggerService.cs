namespace PcbDispatchService.Services;

/// <summary>
/// Сервис логирования. Реализуется просто через консоль, но можно поиграть.
/// </summary>
public class MyCustomLoggerService : IMyCustomLoggerService
{
    /// <summary>
    /// Залогировать куда-угодно.
    /// </summary>
    /// <param name="message">Сообщение логирования.</param>
    public void LogThisSh_t(string message)
    {
        Console.WriteLine(message);
    }
}