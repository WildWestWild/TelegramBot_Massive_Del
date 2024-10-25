using Infrastructure.TelegramBot.BotManagers;
using Infrastructure.TelegramBot.Helpers;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Infrastructure.TelegramBot.Commands;

public class DescriptionCommand: BaseCommand
{
    public DescriptionCommand(ITelegramBotClient botClient, ContextManager contextManager) : base(botClient, contextManager)
    {
    }

    public override bool IsNeedSetEnterCommandText => false;

    public override Task Process(long chatId, CancellationToken token)
    {
        Message = ConstantHelper.DescriptionAboutBot;

        KeyboardMarkup = KeyboardHelper.GetStartKeyboard();

        ParseMode = ParseMode.Html;
        
        AddEventToRemoveContext(token);
        
        return base.Process(chatId, token);
    }
}