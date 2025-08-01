namespace CargoDesk.Application.Services.Telegram;

public interface IRouteNotificationService
{
    Task NotifyDriverRouteCreatedAsync(Guid driverId, Guid routeId, CancellationToken ct);
}