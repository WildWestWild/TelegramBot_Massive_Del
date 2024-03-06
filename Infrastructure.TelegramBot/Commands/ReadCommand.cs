using System.Text;
using System.Text.RegularExpressions;
using Core.ListActions.ActionCommands;
using Core.ListActions.Actions;
using Core.ListActions.DTO;
using Infrastructure.TelegramBot.Extensions;
using Infrastructure.TelegramBot.Helpers;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Infrastructure.TelegramBot.Commands;

public class ReadCommand: BaseCommand
{
    private readonly ReadListAction _readListAction;

    public static Regex FindLastUniqueListPart { get; } = new(
        @"\-G\-(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$",
        RegexOptions.Compiled | RegexOptions.Singleline);
    
    public static Regex FindListInCommandText { get; } = new(
        @"\(.*\-G\-[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}\)",
        RegexOptions.Compiled | RegexOptions.Singleline);
    
    public ReadCommand(ITelegramBotClient botClient, ContextManager contextManager, ReadListAction readListAction) : base(botClient, contextManager)
    {
        _readListAction = readListAction;
    }

    public override bool IsNeedSetEnterCommandText => true;

    public override async Task Process(long chatId, CancellationToken token)
    {
        var uniqueListName = FindListInCommandText
            .Match(EnterCommandText ?? throw new ArgumentNullException(nameof(EnterCommandText)))
            .Value.TrimStart('(').TrimEnd(')');
        
        AfterCommandEvent += async () =>
        {
            await ContextManager.CreateContext(chatId,null, uniqueListName, token);
        };

        ReadListCommand command = new ReadListCommand
        {
            ChatId = chatId,
            Name = uniqueListName
        };

        var list = await _readListAction.GetList(command, token);
        if (list is null)
        {
            Message = @"Данный список не найден!";
            KeyboardMarkup = KeyboardHelper.GetStartKeyboard();
            await base.Process(chatId, token);
            return;
        }
        
        if (!list.Any())
        {
            Message = @"Данный список не может быть отображен, так как в нём нет элементов. Попробуйте их добавить через виртуальную клавиатуру бота!";
            KeyboardMarkup = KeyboardHelper.GetKeyboardForConcreteList(uniqueListName);
            await base.Process(chatId, token);
            return;
        }

        Message = PrepareListInHtmlMessageFormat(list, uniqueListName.GetOnlyListName());
        KeyboardMarkup = KeyboardHelper.GetKeyboardForConcreteList(uniqueListName);
        ParseMode = ParseMode.MarkdownV2;

        await base.Process(chatId, token);
    }

    private string PrepareListInHtmlMessageFormat(UserListElementDTO[] list, string listName)
    {
        var htmlMessage = new StringBuilder($"__{listName}__");
        foreach (var dto in list)
        {
            htmlMessage.Append("\n \n");
            htmlMessage.Append($@" {dto.Number}\. {GetProcessingData(dto)} ");
        }
        
        return htmlMessage.ToString();
    }

    private string GetProcessingData(UserListElementDTO dto)
    {
        if (dto.IsStrikingOut)
        {
            return "~" + dto.Data + "~";
        }

        return dto.Data;
    }
}