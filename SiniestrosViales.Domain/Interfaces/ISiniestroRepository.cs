using SiniestrosViales.Domain.Entities;

namespace SiniestrosViales.Domain.Interfaces;

public interface ISiniestroRepository
{
    Task<Siniestro?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Siniestro>> GetWithFiltersAsync(
        int? departamentoId = null,
        DateTime? fechaInicio = null,
        DateTime? fechaFin = null,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default);
    Task<int> CountAsync(
        int? departamentoId = null,
        DateTime? fechaInicio = null,
        DateTime? fechaFin = null,
        CancellationToken cancellationToken = default);
    Task<Guid> AddAsync(Siniestro siniestro, CancellationToken cancellationToken = default);
}
