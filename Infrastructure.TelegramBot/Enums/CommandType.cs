using System.ComponentModel;

namespace Infrastructure.TelegramBot.Enums;

public enum CommandType
{
    [Description("Создай новый список")]
    CreateNewList,
    [Description("Добавь элемент")]
    AddElement,
    [Description("Удали элемент")]
    DeleteElement,
    [Description("Редактируй элемент")]
    UpdateElement,
    [Description("Вычеркни элемент")]
    StrikingOutElement,
    [Description("Дай ссылку на список")]
    CopyLink,
    [Description("/description")]
    GetDescription,
    [Description("/start")]
    Start,
    [Description("/history")]
    GetHistory
}