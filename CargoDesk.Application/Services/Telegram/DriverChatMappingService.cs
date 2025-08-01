using CargoDesk.Infrastructure.Persistence.Entities.Telegram;

namespace CargoDesk.Application.Services.Telegram;

public class DriverChatMappingService : IDriverChatMappingService
{
    private readonly IDriverChatMappingRepository _repository;

    public DriverChatMappingService(IDriverChatMappingRepository repository)
    {
        _repository = repository;
    }

    public async Task MapAsync(Guid driverId, string phoneNumber, long chatId, CancellationToken ct)
    {
        var norm = new string(phoneNumber.Where(char.IsDigit).ToArray());
        var existing = await _repository.FindByDriverIdAsync(driverId, ct);

        var mapping = new DriverChatMappingEntity
        {
            DriverId = driverId,
            DriverPhone = norm,
            ChatId = chatId
        };

        if (existing is null)
        {
            await _repository.AddAsync(mapping, ct);
        }
        else
        {
            await _repository.UpdateAsync(mapping, ct);
        }
    }

    public async Task<long?> GetChatIdByDriverIdAsync(Guid driverId, CancellationToken ct)
    {
        var m = await _repository.FindByDriverIdAsync(driverId, ct);
        return m?.ChatId;
    }

    public async Task<Guid?> GetDriverIdByChatIdAsync(long chatId, CancellationToken ct)
    {
        var ent = await _repository.FindByChatIdAsync(chatId, ct);
        return ent?.DriverId;
    }
}