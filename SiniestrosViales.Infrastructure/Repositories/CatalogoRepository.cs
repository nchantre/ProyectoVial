using Microsoft.EntityFrameworkCore;
using SiniestrosViales.Domain.Entities;
using SiniestrosViales.Domain.Interfaces;
using SiniestrosViales.Infrastructure.Data.DbContext;

namespace SiniestrosViales.Infrastructure.Repositories;

public class CatalogoRepository : ICatalogoRepository
{
    private readonly SiniestrosVialesDbContext _context;

    public CatalogoRepository(SiniestrosVialesDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TipoSiniestro>> GetTiposSiniestroActivosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.TiposSiniestro
            .Where(t => t.Activo)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Departamento>> GetDepartamentosActivosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Departamentos
            .Where(d => d.Activo)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Ciudad>> GetCiudadesActivasAsync(int? departamentoId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Ciudades.Where(c => c.Activo);

        if (departamentoId.HasValue)
        {
            query = query.Where(c => c.DepartamentoId == departamentoId.Value);
        }

        return await query.ToListAsync(cancellationToken);
    }
}
