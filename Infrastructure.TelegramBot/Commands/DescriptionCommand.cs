using Telegram.Bot;
using Telegram.Bot.Types;

namespace Infrastructure.TelegramBot.Commands;

public class DescriptionCommand: BaseCommand
{
    public DescriptionCommand(ITelegramBotClient botClient) : base(botClient)
    {
    }

    public override Task Process(Update update, CancellationToken token)
    {
        Message = "Приветствую! \"Массив дел\" на связи! \n \n " +
                  "Я - бот, который поможет хранить и редактировать упорядоченные списки: \n покупки в магазине, топ любимых сериалов, вещей, которые нужно взять в поход и т.д. " +
                  "Абслолютно всё, что угодно! Моя цель сделать это просто и удобно для вас! \n \n Если у вас уже появилась идея, смело жмите на \"Создай список\" и запускайте меня, далее вам всё сразу станет понятным. \n \n" +
                  "Если вы захотите поделиться списком с друзьями, нажмите кнопку \"Скопируй ссылку на список\" и отправьте эту ссылку в чате в телеграм. Все, кто перейдут по ней получат от меня ваш список и возможность его редактировать.";
        
        return base.Process(update, token);
    }
}