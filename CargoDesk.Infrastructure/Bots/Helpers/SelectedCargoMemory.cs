using System.Collections.Concurrent;

namespace CargoDesk.Infrastructure.Bots.Helpers;

public static class SelectedCargoMemory
{
    public static ConcurrentDictionary<long, Guid> SelectedCargoByChatId { get; } = new();
}