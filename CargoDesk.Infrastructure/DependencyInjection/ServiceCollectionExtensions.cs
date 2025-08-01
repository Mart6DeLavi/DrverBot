using CargoDesk.Application.Mappings;
using CargoDesk.Application.Services;
using CargoDesk.Application.Services.Telegram;
using CargoDesk.Domain.Interfaces.Services;
using CargoDesk.Infrastructure.Bots;
using CargoDesk.Infrastructure.Persistence.Context;
using CargoDesk.Infrastructure.Persistence.Repositories;
using CargoDesk.Infrastructure.Persistence.Repositories.Telegram;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace CargoDesk.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenApi();

        services.AddDbContext<DatabaseContext>(opts =>
            opts.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                .UseSnakeCaseNamingConvention()
            );

        services.AddAutoMapper(typeof(GenericMappingProfile).Assembly);

        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<ICargoRepository,   CargoRepository>();
        services.AddScoped<IDriverRepository,  DriverRepository>();
        services.AddScoped<IRouteRepository,   RouteRepository>();
        services.AddScoped<IDelayRequestRepository, DelayRequestRepository>();

        services.AddScoped<IAddressService, AddressService>();
        services.AddScoped<ICargoService, CargoService>();
        services.AddScoped<IDriverService, DriverService>();
        services.AddScoped<IRouteService, RouteService>();
        services.AddScoped<IDriverWorkSessionRepository, DriverWorkSessionRepository>();
        services.AddScoped<IRouteCargoStatusRepository, RouteCargoStatusRepository>();

        return services;
    }

    public static IServiceCollection AddTelegramBot(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ITelegramBotClient>(_ => new TelegramBotClient(configuration["Telegram:BotToken"]));
        services.AddSingleton<MenuCommandInitializer>();

        services.AddHostedService<TelegramBotHostedService>();

        services.AddTransient<IUpdateHandler, StartUpdateHandler>();
        services.AddTransient<IUpdateHandler, ContactUpdateHandler>();
        services.AddTransient<IUpdateHandler, RouteStartCallbackHandler>();
        services.AddTransient<IUpdateHandler, StatusUpdateHandler>();
        services.AddTransient<IUpdateHandler, ReportIssueRequestHandler>();
        services.AddTransient<IUpdateHandler, IssueTypeCallbackHandler>();
        services.AddTransient<IUpdateHandler, IssueProofHandler>();
        services.AddTransient<IUpdateHandler, DelayRequestHandler>();
        services.AddTransient<IUpdateHandler, CancelStatusChangeHandler>();
        services.AddTransient<IUpdateHandler, FinishWorkHandler>();
        services.AddTransient<IUpdateHandler, ResumeWorkHandler>();
        services.AddTransient<IUpdateHandler, CargoSelectionHandler>();
        services.AddTransient<IUpdateHandler, BackToAllCargosHandler>();
        services.AddTransient<IUpdateHandler, ShowCargoInfoHandler>();
        services.AddTransient<IUpdateHandler, InstructionCommandHandler>();
        services.AddTransient<IUpdateHandler, FallbackHandler>();

        services.AddTransient<IRouteNotificationService, RouteNotificationService>();

        services.AddScoped<IDriverChatMappingRepository, DriverChatMappingRepository>();
        services.AddScoped<IDriverChatMappingService, DriverChatMappingService>();
        services.AddScoped<IIssueRepository, IssueRepository>();

        return services;
    }
}