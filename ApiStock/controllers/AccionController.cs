using Microsoft.AspNetCore.Mvc;

class AccionController : ControllerBase
{
    IAccionService _accionService;

    public AccionController(IAccionService accionService)
    {
        _accionService = accionService;
    }
}