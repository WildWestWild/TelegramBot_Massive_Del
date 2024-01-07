using Telegram.Bot;
using Telegram.Bot.Types;

namespace Infrastructure.TelegramBot.Commands;

public class NotFoundCommand: BaseCommand
{
    public NotFoundCommand(ITelegramBotClient botClient) : base(botClient)
    {
    }
    
    public override Task Process(Update update, CancellationToken token)
    {
        Message =
            $"Извините, но \"{update.Message!.Text}\" - это неизвестная для меня команда, пожалуйста, введите команду из меню, либо с виртуальной клавиатуры.";
        
        return base.Process(update, token);
    }
}