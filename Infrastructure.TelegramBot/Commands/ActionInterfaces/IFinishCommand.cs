namespace Infrastructure.TelegramBot.Commands.ActionInterfaces;

public interface IFinishCommand
{
    Task FinishCommandOnCancel(long chatId, CancellationToken token);
}