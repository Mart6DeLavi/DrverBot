using CargoDesk.Application.Services.Telegram;
using CargoDesk.Infrastructure.Persistence.Entities.Telegram;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CargoDesk.Infrastructure.Bots;

public class IssueTypeCallbackHandler : IUpdateHandler
{

    private static readonly HashSet<long> _waitingProofChats = new();

    private readonly IDriverChatMappingService _chatMap;
    private readonly IIssueRepository _issueRepo;

    public IssueTypeCallbackHandler(IDriverChatMappingService chatMap, IIssueRepository issueRepo)
    {
        _chatMap = chatMap;
        _issueRepo = issueRepo;
    }

    public Task<bool> CanHandleAsync(Update update, CancellationToken ct)
    {
        return Task.FromResult(
            update.Type == UpdateType.CallbackQuery &&
            update.CallbackQuery?.Data?.StartsWith("issue:") == true
        );
    }

    public async Task HandleAsync(ITelegramBotClient bot, Update update, CancellationToken ct)
    {
        var cq        = update.CallbackQuery!;
        var chatId    = cq.Message!.Chat.Id;
        var issueType = cq.Data!.Split(':', 2)[1];

        await bot.AnswerCallbackQuery(cq.Id, $"You selected: {issueType}", cancellationToken: ct);

        await bot.SendMessage(
            chatId:      chatId,
            text:        "Please send any photos, videos, voice or text messages as proof within 3 minutes.",
            cancellationToken: ct
        );

        var driverId = await _chatMap.GetDriverIdByChatIdAsync(chatId, ct);
        if (driverId is not null)
        {
            var issue = new IssueEntity
            {
                Id = Guid.NewGuid(),
                DriverId = driverId.Value,
                ChatId = chatId,
                IssueType = issueType,
                DispatcherId = Guid.Empty,
                CreatedAt = DateTime.UtcNow
            };
            await _issueRepo.CreateAsync(issue, ct);
        }

        _waitingProofChats.Add(chatId);
    }

    public static bool IsWaitingProof(long chatId)
    {
        return _waitingProofChats.Contains(chatId);
    }

    public static void ClearWaitingProof(long chatId)
    {
        _waitingProofChats.Remove(chatId);
    }
}