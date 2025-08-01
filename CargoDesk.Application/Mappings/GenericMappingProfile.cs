using System.Reflection;
using AutoMapper;

namespace CargoDesk.Application.Mappings;

public class GenericMappingProfile : Profile
{
    public GenericMappingProfile()
    {
        var asm = Assembly.GetExecutingAssembly();
        var maps = asm.GetTypes()
            .SelectMany(t => t.GetInterfaces(), (t, i) => (DTOs: t, If: i))
            .Where(x => x.If.IsGenericType && x.If.GetGenericTypeDefinition() == typeof(IMapTo<>));

        foreach (var (dto, iface) in maps )
        {
            var entity = iface.GetGenericArguments()[0];
            CreateMap(dto, entity);
        }
    }
}