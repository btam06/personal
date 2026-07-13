using Autofac;
using Microsoft.EntityFrameworkCore;

public class SourceModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterGeneric(typeof(BaseRepository<>))
            .As(typeof(BaseRepository<>))
            .InstancePerLifetimeScope()
        ;

        builder.RegisterType<Items>().AsImplementedInterfaces().InstancePerLifetimeScope();
    }
}