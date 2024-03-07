using System.Text;
using Infrastructure.Storage.Models;
using Infrastructure.TelegramBot.BotManagers;
using Infrastructure.TelegramBot.Extensions;
using Infrastructure.TelegramBot.Helpers;
using Telegram.Bot;

namespace Infrastructure.TelegramBot.Commands;

public class GetHistoryCommand: BaseCommand
{
    private readonly HistoryManager _historyManager;

    public GetHistoryCommand(ITelegramBotClient botClient, HistoryManager historyManager, ContextManager contextManager) : base(botClient, contextManager)
    {
        _historyManager = historyManager;
    }

    public override bool IsNeedSetEnterCommandText => false;

    public override async Task Process(long chatId, CancellationToken token)
    {
        var userListHistories = await _historyManager.GetHistory(chatId, token);
        Message = PrepareUserListHistoriesToMarkdownV2Message(userListHistories);
        KeyboardMarkup = KeyboardHelper.GetCancelKeyboard();
        
        await base.Process(chatId, token);
    }

    private string PrepareUserListHistoriesToMarkdownV2Message(UserListHistory[] userListHistories)
    {
        StringBuilder message = new StringBuilder();
        foreach (var item in userListHistories)
        {
            message.Append($"Название: {item.ListName.GetOnlyListName()} \n");
            message.Append($"Последнее использование: {item.LastUseDate.ToString("u")} \n");
            message.Append($"Ссылка: {item.ListName.GetLink()} \n \n");
        }

        return message.ToString();
    }
}