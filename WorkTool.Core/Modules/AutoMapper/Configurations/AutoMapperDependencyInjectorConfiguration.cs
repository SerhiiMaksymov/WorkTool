using AutoMapper;

using WorkTool.Core.Modules.AutoMapper.Extensions;

namespace WorkTool.Core.Modules.AutoMapper.Configurations;

public readonly struct AutoMapperDependencyInjectorConfiguration : IDependencyInjectorConfiguration
{
    public void Configure(IDependencyInjectorRegister register)
    {
        register.RegisterTransient(
            () => new MapperConfiguration(cfg => cfg.AddProfilesFormAssemblies())
        );

        register.RegisterTransient<IMapper>(
            (MapperConfiguration configuration) => configuration.CreateMapper()
        );
    }
}
