using Microsoft.AspNetCore.Mvc;
using PcbDispatchService.Services;

namespace PcbDispatchService.Controllers;

public class PcbController : Controller
{
    private readonly PcbFactory _pcbFactory;

    public PcbController(PcbFactory pcbFactory)
    {
        _pcbFactory = pcbFactory;
    }

    // GET
    public IActionResult Index()
    {
        
    }
}