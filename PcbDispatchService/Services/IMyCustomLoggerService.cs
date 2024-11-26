namespace PcbDispatchService.Services;

/// <summary>
/// Сервис логирования.
/// </summary>
public interface IMyCustomLoggerService
{
    /// <summary>
    /// Залогировать куда-угодно.
    /// </summary>
    /// <param name="message">Сообщение логирования.</param>
    void LogThisSh_t(string message);
}