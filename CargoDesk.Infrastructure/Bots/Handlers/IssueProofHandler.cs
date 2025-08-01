using CargoDesk.Application.Services.Telegram;
using CargoDesk.Infrastructure.Persistence.Entities.Telegram;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CargoDesk.Infrastructure.Bots;

public class IssueProofHandler : IUpdateHandler
{
    private readonly IDriverChatMappingService _chatMap;
    private readonly IIssueRepository _issueRepo;

    private static readonly MessageType[] AllowedTypes = {
        MessageType.Photo,
        MessageType.Video,
        MessageType.Voice,
        MessageType.Text
    };

    public IssueProofHandler(IDriverChatMappingService chatMap, IIssueRepository issueRepo)
    {
        _chatMap = chatMap;
        _issueRepo = issueRepo;
    }

    public async Task<bool> CanHandleAsync(Update update, CancellationToken ct)
    {
        if (update.Type != UpdateType.Message || update.Message?.Chat == null)
            return false;

        var chatId = update.Message.Chat.Id;

        var driverId = await _chatMap.GetDriverIdByChatIdAsync(chatId, ct);
        if (driverId is null)
            return false;

        return IssueTypeCallbackHandler.IsWaitingProof(chatId);
    }

    public async Task HandleAsync(ITelegramBotClient bot, Update update, CancellationToken ct)
    {
        var msg    = update.Message!;
        var chatId = msg.Chat.Id;


        if (!AllowedTypes.Contains(msg.Type))
        {
            await bot.SendMessage(
                chatId: chatId,
                text:   "❗Unsupported data type. Please send a photo, video, voice message or text.",
                cancellationToken: ct
            );
            return;
        }

        var issue = await _issueRepo.GetByChatIdAsync(chatId, ct);
        if (issue is null)
        {
            IssueTypeCallbackHandler.ClearWaitingProof(chatId);
            return;
        }

        var proof = new IssueProofEntity
        {
            Id          = Guid.NewGuid(),
            IssueId     = issue.Id,
            MessageType = msg.Type.ToString(),
            Content     = msg.Type switch
            {
                MessageType.Photo => msg.Photo!.Last().FileId,
                MessageType.Video => msg.Video!.FileId,
                MessageType.Voice => msg.Voice!.FileId,
                MessageType.Text  => msg.Text   ?? string.Empty,
                _                 => string.Empty
            },
            CreatedAt   = DateTime.UtcNow
        };
        await _issueRepo.AddProofAsync(proof, ct);

        await bot.SendMessage(
            chatId: chatId,
            text:   "Thanks! Your confirmation has been received and saved.",
            cancellationToken: ct
        );

        IssueTypeCallbackHandler.ClearWaitingProof(chatId);
    }
}