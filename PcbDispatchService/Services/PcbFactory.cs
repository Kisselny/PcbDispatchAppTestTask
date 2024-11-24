namespace PcbDispatchService.Services;

public class PcbFactory
{
    private readonly IServiceProvider _serviceProvider;

    public PcbFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    void awedi()
    {
        var sidsd = _serviceProvider.GetService<QualityControlService>();
    }
}