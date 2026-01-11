using MediatR;
using Microsoft.AspNetCore.Mvc;
using SiniestrosViales.Application.Commands.Siniestros;
using SiniestrosViales.Application.DTOs;
using SiniestrosViales.Application.Queries.Siniestros;

namespace SiniestrosViales.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SiniestrosController : ControllerBase
{
    private readonly IMediator _mediator;

    public SiniestrosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Crear un nuevo siniestro vial
    /// </summary>
    /// <param name="dto">Datos del siniestro a crear</param>
    /// <returns>ID del siniestro creado</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Guid>> CreateSiniestro([FromBody] CreateSiniestroDto dto)
    {
        var command = new CreateSiniestroCommand { Siniestro = dto };
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetSiniestroById), new { id }, id);
    }

    /// <summary>
    /// Obtener lista de siniestros con filtros opcionales y paginación
    /// </summary>
    /// <param name="departamentoId">ID del departamento (opcional)</param>
    /// <param name="fechaInicio">Fecha inicial del rango (opcional)</param>
    /// <param name="fechaFin">Fecha final del rango (opcional)</param>
    /// <param name="pageNumber">Número de página (default: 1)</param>
    /// <param name="pageSize">Tamaño de página (default: 10)</param>
    /// <returns>Lista paginada de siniestros</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<SiniestroDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<SiniestroDto>>> GetSiniestros(
        [FromQuery] int? departamentoId = null,
        [FromQuery] DateTime? fechaInicio = null,
        [FromQuery] DateTime? fechaFin = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = new GetSiniestrosQuery
        {
            DepartamentoId = departamentoId,
            FechaInicio = fechaInicio,
            FechaFin = fechaFin,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Obtener un siniestro por su ID
    /// </summary>
    /// <param name="id">ID del siniestro</param>
    /// <returns>Datos del siniestro</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(SiniestroDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SiniestroDto>> GetSiniestroById(Guid id)
    {
        var query = new GetSiniestroByIdQuery { Id = id };
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }
}
