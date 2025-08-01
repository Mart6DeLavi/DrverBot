using CargoDesk.Infrastructure.Persistence.Enums;

namespace CargoDesk.Infrastructure.Bots.Helpers;

public static class StatusEmoji
{
    private static readonly IReadOnlyDictionary<RouteStatus, string> _emoji = new Dictionary<RouteStatus, string>
    {
        [RouteStatus.Assigned]            = "🟢",
        [RouteStatus.InTransitToPickup]   = "🟡",
        [RouteStatus.AtPickupPoint]       = "🔵",
        [RouteStatus.Loading]             = "⚙️",
        [RouteStatus.LoadingCompleted]    = "✅",
        [RouteStatus.AtDeliveryPoint]     = "🔷",
        [RouteStatus.Unloading]           = "📦",
        [RouteStatus.UnloadingCompleted]  = "🏁"
    };

    public static string Get(RouteStatus status)
        => _emoji.TryGetValue(status, out var emoji) ? emoji : "";

    public static IReadOnlyCollection<string> AllEmojis => _emoji.Values.ToList();

    public static bool TryGetStatus(string emoji, out RouteStatus status)
    {
        foreach (var kv in _emoji)
        {
            if (kv.Value == emoji)
            {
                status = kv.Key;
                return true;
            }
        }
        status = default;
        return false;
    }
}