using Serilog;
using TogetherBoardsApp.Backend.Application;
using TogetherBoardsApp.Backend.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseInfrastructure();

app.Run();

public partial class Program;