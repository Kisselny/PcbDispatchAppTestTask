namespace PcbDispatchService.Domain.Logic;

public sealed class BusinessException(string message, Exception innerException = null)
    : Exception(message, innerException);