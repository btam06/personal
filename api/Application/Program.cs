using Microsoft.EntityFrameworkCore;
using Autofac.Extensions.DependencyInjection;
using Autofac;

var builder = WebApplication.CreateBuilder(args);

// Libraries
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddLibraries(builder.Configuration);

// DI configuration
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{
    container.RegisterModule(new DataModule(builder.Configuration));
    container.RegisterModule(new SourceModule());
});

// Services
builder.Services.AddAuth(builder.Configuration);

// Build application

var app = builder.Build();

// Development settings
if (app.Environment.IsDevelopment()) {
    app.MapOpenApi();

} else {
    app.UseHttpsRedirection();
}

app.MapControllers();

app.Run();

public partial class Program { }
