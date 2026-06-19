using Autofac;
using Microsoft.EntityFrameworkCore;

public class DataModule : Module
{
    private readonly IConfiguration _config;

    public DataModule(IConfiguration config)
    {
        _config = config;    
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(context =>
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(_config.GetConnectionString("Default"))
                .Options;

            return new AppDbContext(options);
        });
    } 
}