namespace PcbDispatchService.Domain.Logic;

/// <summary>
/// Представляет бизнес-исключение.
/// </summary>
/// <param name="message">Сообщение исключения.</param>
/// <param name="innerException"></param>
public sealed class BusinessException(string message, Exception innerException = null)
    : Exception(message, innerException);