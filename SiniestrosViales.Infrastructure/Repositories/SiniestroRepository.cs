using Microsoft.EntityFrameworkCore;
using SiniestrosViales.Domain.Entities;
using SiniestrosViales.Domain.Interfaces;
using SiniestrosViales.Infrastructure.Data.DbContext;

namespace SiniestrosViales.Infrastructure.Repositories;

public class SiniestroRepository : ISiniestroRepository
{
    private readonly SiniestrosVialesDbContext _context;

    public SiniestroRepository(SiniestrosVialesDbContext context)
    {
        _context = context;
    }

    public async Task<Siniestro?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Siniestros
            .Include(s => s.Departamento)
            .Include(s => s.Ciudad)
            .Include(s => s.TipoSiniestro)
            .Include(s => s.Vehiculos)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Siniestro>> GetWithFiltersAsync(
        int? departamentoId = null,
        DateTime? fechaInicio = null,
        DateTime? fechaFin = null,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Siniestros
            .Include(s => s.Departamento)
            .Include(s => s.Ciudad)
            .Include(s => s.TipoSiniestro)
            .Include(s => s.Vehiculos)
            .AsQueryable();

        if (departamentoId.HasValue)
        {
            query = query.Where(s => s.DepartamentoId == departamentoId.Value);
        }

        if (fechaInicio.HasValue)
        {
            query = query.Where(s => s.FechaHora >= fechaInicio.Value);
        }

        if (fechaFin.HasValue)
        {
            query = query.Where(s => s.FechaHora <= fechaFin.Value);
        }

        return await query
            .OrderByDescending(s => s.FechaHora)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(
        int? departamentoId = null,
        DateTime? fechaInicio = null,
        DateTime? fechaFin = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Siniestros.AsQueryable();

        if (departamentoId.HasValue)
        {
            query = query.Where(s => s.DepartamentoId == departamentoId.Value);
        }

        if (fechaInicio.HasValue)
        {
            query = query.Where(s => s.FechaHora >= fechaInicio.Value);
        }

        if (fechaFin.HasValue)
        {
            query = query.Where(s => s.FechaHora <= fechaFin.Value);
        }

        return await query.CountAsync(cancellationToken);
    }

    public async Task<Guid> AddAsync(Siniestro siniestro, CancellationToken cancellationToken = default)
    {
        // Guardar los vehículos temporalmente
        var vehiculos = siniestro.Vehiculos.ToList();
        
        // Crear una nueva instancia de Siniestro solo con los datos necesarios (sin propiedades de navegación)
        // Esto evita que EF Core intente insertar las propiedades de navegación
        var siniestroToAdd = new Siniestro(
            siniestro.FechaHora,
            siniestro.DepartamentoId,
            siniestro.CiudadId,
            siniestro.TipoSiniestroId,
            siniestro.NumeroVictimas,
            siniestro.Descripcion);
        
        // El ID se generará automáticamente en el constructor de Entity
        
        // Agregar el siniestro (sin propiedades de navegación ni vehículos)
        await _context.Siniestros.AddAsync(siniestroToAdd, cancellationToken);
        
        // Guardar primero el siniestro para obtener su ID
        await _context.SaveChangesAsync(cancellationToken);
        
        // Ahora agregar los vehículos con el SiniestroId correcto
        foreach (var vehiculo in vehiculos)
        {
            _context.Entry(vehiculo).Property("SiniestroId").CurrentValue = siniestroToAdd.Id;
            await _context.VehiculosInvolucrados.AddAsync(vehiculo, cancellationToken);
        }
        
        // Guardar los vehículos
        await _context.SaveChangesAsync(cancellationToken);
        
        return siniestroToAdd.Id;
    }
}
