using Autofac;
using Microsoft.EntityFrameworkCore;

public class SourceModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterGeneric(typeof(Repository<>))
            .As(typeof(Repository<>))
            .InstancePerLifetimeScope()
        ;
    }
}