using CargoDesk.Infrastructure.Persistence.Enums;

namespace CargoDesk.Infrastructure.Bots.Helpers;

public static class StatusDisplay
{
    public static readonly IReadOnlyDictionary<RouteStatus, string> Display =
        new Dictionary<RouteStatus, string>
        {
            [RouteStatus.Assigned]            = "🟢 Assigned",
            [RouteStatus.InTransitToPickup]   = "🟡 In Transit To Pickup",
            [RouteStatus.AtPickupPoint]       = "🔵 At Pickup Point",
            [RouteStatus.Loading]             = "⚙️ Loading",
            [RouteStatus.LoadingCompleted]    = "✅ Loading Completed",
            [RouteStatus.AtDeliveryPoint]     = "🔷 At Delivery Point",
            [RouteStatus.Unloading]           = "📦 Unloading",
            [RouteStatus.UnloadingCompleted]  = "🏁 Unloading Completed"
        };

    public static readonly IReadOnlyDictionary<string, RouteStatus> Lookup =
        Display.ToDictionary(kv => kv.Value, kv => kv.Key, StringComparer.Ordinal);
}