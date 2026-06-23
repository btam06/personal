using Microsoft.EntityFrameworkCore;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using Mapster;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddMapster();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{
    container.RegisterModule(new DataModule(builder.Configuration));
    container.RegisterModule(new SourceModule());
});

var app = builder.Build();

// Development settings
if (app.Environment.IsDevelopment()) {
    app.MapOpenApi();

} else {
    app.UseHttpsRedirection();
}

app.MapControllers();

app.Run();
