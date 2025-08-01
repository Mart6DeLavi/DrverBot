using CargoDesk.Infrastructure.Persistence.Entities;
using CargoDesk.Infrastructure.Persistence.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace CargoDesk.Infrastructure.Bots.Helpers;

public class KeyboardHelpers
{
    public static ReplyKeyboardMarkup BuildMainCargoKeyboard(List<(CargoEntity Cargo, RouteStatus route)> cargos)
    {
        var rows = new List<KeyboardButton[]>();
        var currentRow = new List<KeyboardButton>();

        foreach (var (cargo, status) in cargos)
        {
            var emoji = StatusEmoji.Get(status);
            var button = new KeyboardButton($"{emoji} {cargo.ReferenceNumber}");

            currentRow.Add(button);
            if (currentRow.Count == 3)
            {
                rows.Add(currentRow.ToArray());
                currentRow = new List<KeyboardButton>();
            }
        }


        if (currentRow.Count > 0)
        {
            rows.Add(currentRow.ToArray());
        }

        rows.Add(new []
        {
            new KeyboardButton("⚠️ Report issue"),
            new KeyboardButton("⏸️ Finish work")
        });

        return new ReplyKeyboardMarkup(rows)
        {
            ResizeKeyboard = true,
            OneTimeKeyboard = false
        };
    }

    public static ReplyKeyboardMarkup BuildCargoStatusKeyboard(RouteStatus status)
    {
        if (status == RouteStatus.UnloadingCompleted)
        {
            return new ReplyKeyboardMarkup(new[]
            {
                new[] { new KeyboardButton("⬅️ Back to all cargos") }
            })
            {
                ResizeKeyboard  = true,
                OneTimeKeyboard = false
            };
        }

        var label = StatusDisplay.Display[status];

        return new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton(label) },

            new[]
            {
                new KeyboardButton("⬅️ Back to all cargos"),
                new KeyboardButton("🔍 Cargo info")
            },

            new[]
            {
                new KeyboardButton("⚠️ Report issue"),
                new KeyboardButton("⏸️ Finish work")
            }
        })
        {
            ResizeKeyboard  = true,
            OneTimeKeyboard = false
        };
    }
}