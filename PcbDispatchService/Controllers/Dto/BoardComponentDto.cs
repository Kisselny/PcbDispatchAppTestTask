namespace PcbDispatchService.Controllers.Dto;

/// <summary>
/// Описание компонента для добавления на плату.
/// </summary>
/// <param name="ComponentTypeName">Название типа компонента.</param>
/// <param name="Quantity">Количество компонента.</param>
public record BoardComponentDto(string ComponentTypeName, int Quantity);