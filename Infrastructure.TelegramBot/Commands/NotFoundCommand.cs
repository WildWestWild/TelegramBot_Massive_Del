using Telegram.Bot;

namespace Infrastructure.TelegramBot.Commands;

public class NotFoundCommand: BaseCommand
{
    public NotFoundCommand(ITelegramBotClient botClient, ContextManager contextManager) : base(botClient, contextManager)
    {
    }

    public override bool IsNeedSetEnterCommandText => false;

    public override Task Process(long chatId, CancellationToken token)
    {
        Message =
            "Извините, но это неизвестная для меня команда, пожалуйста, введите команду из меню, либо с виртуальной клавиатуры.";
        
        return base.Process(chatId, token);
    }
}