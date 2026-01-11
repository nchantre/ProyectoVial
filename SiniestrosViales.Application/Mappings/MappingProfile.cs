using AutoMapper;
using SiniestrosViales.Application.DTOs;
using SiniestrosViales.Domain.Entities;
using SiniestrosViales.Domain.ValueObjects;

namespace SiniestrosViales.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Siniestro → SiniestroDto
        CreateMap<Siniestro, SiniestroDto>()
            .ForMember(dest => dest.Departamento, opt => opt.MapFrom(src => src.Departamento))
            .ForMember(dest => dest.Ciudad, opt => opt.MapFrom(src => src.Ciudad))
            .ForMember(dest => dest.TipoSiniestro, opt => opt.MapFrom(src => src.TipoSiniestro))
            .ForMember(dest => dest.Vehiculos, opt => opt.MapFrom(src => src.Vehiculos));

        // Departamento → DepartamentoDto
        CreateMap<Departamento, DepartamentoDto>();

        // Ciudad → CiudadDto
        CreateMap<Ciudad, CiudadDto>();

        // TipoSiniestro → TipoSiniestroDto
        CreateMap<TipoSiniestro, TipoSiniestroDto>();

        // VehiculoInvolucrado → VehiculoInvolucradoDto
        CreateMap<VehiculoInvolucrado, VehiculoInvolucradoDto>();
    }
}
