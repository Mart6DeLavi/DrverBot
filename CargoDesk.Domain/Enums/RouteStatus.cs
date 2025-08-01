using System.Text.Json.Serialization;

namespace CargoDesk.Infrastructure.Persistence.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RouteStatus
{
    Assigned,
    InTransitToPickup,
    AtPickupPoint,
    Loading,
    LoadingCompleted,
    AtDeliveryPoint,
    Unloading,
    UnloadingCompleted
}