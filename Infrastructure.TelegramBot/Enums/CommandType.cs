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
    [Description("Скопируй ссылку")]
    CopyLink,
    [Description("/description")]
    GetDescription,
    [Description("/start")]
    Start
}