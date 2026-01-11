using SiniestrosViales.Domain.Entities;

namespace SiniestrosViales.Domain.Interfaces;

public interface ICatalogoRepository
{
    Task<IEnumerable<TipoSiniestro>> GetTiposSiniestroActivosAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Departamento>> GetDepartamentosActivosAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Ciudad>> GetCiudadesActivasAsync(int? departamentoId = null, CancellationToken cancellationToken = default);
}
