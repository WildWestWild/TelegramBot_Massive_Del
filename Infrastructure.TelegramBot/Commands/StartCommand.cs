﻿using Infrastructure.TelegramBot.Helpers;
using Telegram.Bot;

namespace Infrastructure.TelegramBot.Commands;

public class StartCommand: BaseCommand
{
    public StartCommand(ITelegramBotClient botClient, ContextManager contextManager) : base(botClient, contextManager)
    {
    }

    public override bool IsNeedSetEnterCommandText => false;

    public override Task Process(long chatId, CancellationToken token)
    {
        Message = "Давайте начнём!";

        KeyboardMarkup = KeyboardHelper.GetStartKeyboard();
        
        AddEventToRemoveContext(token);
        
        return base.Process(chatId, token);
    }
}