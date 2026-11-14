using eCommerce.Application;
using eCommerce.Infrastructure;
using eCommerce.Messaging.Worker.Consumers;
using eCommerce.Messaging.Worker.HostedServices;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSerilog((services, configuration) => configuration
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console());

builder.Services.AddApplication();

builder.Services.AddInfrastructure(
    builder.Configuration.GetConnectionString("DatabaseConnection")!);

builder.Services.AddTransient<IOrderCreatedAction, EmailNotificationAction>();
builder.Services.AddTransient<IOrderCreatedAction, InventoryManagementAction>();
builder.Services.AddTransient<IOrderCreatedAction, OrderAuditAction>();

builder.Services.AddHostedService<OrderEventProcessor>();

var host = builder.Build();
host.Run();
