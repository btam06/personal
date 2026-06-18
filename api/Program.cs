using Microsoft.EntityFrameworkCore;
//using Autofac.Extensions.DependencyInjection;

var builder    = WebApplication.CreateBuilder(args);
var app        = builder.Build();

builder.Services.AddOpenApi();

//builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
