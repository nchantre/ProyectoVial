using MediatR;
using Microsoft.AspNetCore.Mvc;
using SiniestrosViales.Application.Queries.Catalogos;

namespace SiniestrosViales.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CatalogosController : ControllerBase
{
    private readonly IMediator _mediator;

    public CatalogosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtener todos los tipos de siniestro
    /// </summary>
    [HttpGet("tipos-siniestro")]
    [ProducesResponseType(typeof(List<SiniestrosViales.Application.DTOs.TipoSiniestroDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<SiniestrosViales.Application.DTOs.TipoSiniestroDto>>> GetTiposSiniestro()
    {
        var query = new GetTiposSiniestroQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Obtener todos los departamentos
    /// </summary>
    [HttpGet("departamentos")]
    [ProducesResponseType(typeof(List<SiniestrosViales.Application.DTOs.DepartamentoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<SiniestrosViales.Application.DTOs.DepartamentoDto>>> GetDepartamentos()
    {
        var query = new GetDepartamentosQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Obtener ciudades por departamento
    /// </summary>
    [HttpGet("ciudades")]
    [ProducesResponseType(typeof(List<SiniestrosViales.Application.DTOs.CiudadDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<SiniestrosViales.Application.DTOs.CiudadDto>>> GetCiudades([FromQuery] int? departamentoId = null)
    {
        var query = new GetCiudadesQuery(departamentoId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
