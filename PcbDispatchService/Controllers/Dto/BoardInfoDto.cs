namespace PcbDispatchService.Controllers.Dto;

public record BoardInfoDto(int Id, string Name, int ComponentNumber, string CurrentStatus, string QualityControlStatus);