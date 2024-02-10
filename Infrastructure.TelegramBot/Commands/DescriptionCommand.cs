using Infrastructure.TelegramBot.Helpers;
using Telegram.Bot;

namespace Infrastructure.TelegramBot.Commands;

public class DescriptionCommand: BaseCommand
{
    public DescriptionCommand(ITelegramBotClient botClient, ContextManager contextManager) : base(botClient, contextManager)
    {
    }

    public override bool IsNeedSetEnterCommandText => false;

    public override Task Process(long chatId, CancellationToken token)
    {
        Message = "Приветствую! \"Массив дел\" на связи! \n \n " +
                  "Я - бот, который поможет хранить и редактировать упорядоченные списки: \n покупки в магазине, топ любимых сериалов, вещей, которые нужно взять в поход и т.д. " +
                  "Абслолютно всё, что угодно! Моя цель сделать это просто и удобно для вас! \n \n Если у вас уже появилась идея, смело жмите на \"Создай список\" и запускайте меня, далее вам всё сразу станет понятным. \n \n" +
                  "Если вы захотите поделиться списком с друзьями, нажмите кнопку \"Скопируй ссылку на список\" и отправьте эту ссылку в чате в телеграм. Все, кто перейдут по ней получат от меня ваш список и возможность его редактировать.";

        KeyboardMarkup = KeyboardHelper.GetKeyboard();
        
        AddEventToRemoveContext(token);
        
        return base.Process(chatId, token);
    }
}