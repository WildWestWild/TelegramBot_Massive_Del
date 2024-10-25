namespace Infrastructure.TelegramBot.Helpers;

public static class ConstantHelper
{
    #region AddCommandMessages

    public static readonly string CannotAddElemInAddCommand = "Невозможно добавить элемент, слишком много символов в списке!";

    public static readonly string InputElemInAddCommand =
        "Введите элемент (чтобы выйти из режима записи в список, нажмите 'Завершить действие'):";
    
    public static readonly string SuccessAddElemInAddCommand =
        "Элемент добавлен!";

    public static readonly string ErrorCommandElemInAddCommand = "Ошибка! Элемент не был добавлен.";

    #endregion

    #region CancelCommandMessages

    public static readonly string DoneActionInCancelCommand =  "Действие завершено!";

    #endregion

    #region CreateListCommandMessages

    public static readonly string InputListNameInCreateListCommand =  "Введите название списка: ";

    public static string SuccessCreatingInCreateListCommand(string uniqueListName) =>
        $"Список с названием '{uniqueListName}' успешно создан! \n(В списке не может быть более 15-ти тысяч символов)";
    
    public static string ErrorCreatingInCreateListCommand(string enterCommandText) =>
        $"Ошибка! Список '{enterCommandText}' не был добавлен. Возможно стоит попробовать ещё раз.";

    #endregion

    #region DeleteCommandMessage

    public static readonly string InputListNameInDeleteListCommand =  "Введите название списка: ";
    
    public static readonly string IncorrectNumberInDeleteListCommand =  "Некорректный номер элемента! ";
    
    public static readonly string PickNumberInDeleteListCommand =  "Элемент выбран для удаления!";
    
    public static readonly string DoneActionInDeleteListCommand =   "Элемент удалён!";

    #endregion

    #region DescriptionCommandMessages

    public static readonly string DescriptionAboutBot =  "Приветствую! \"Массив дел\" на связи! \n \n" +
        "Я - бот, который поможет хранить и редактировать упорядоченные списки: \nпокупки в магазине, топ любимых сериалов, вещей, которые нужно взять в поход и т.д. " +
        "Абслолютно всё, что угодно! Моя цель сделать это просто и удобно для вас! \n \nЕсли у вас уже появилась идея, смело жмите на \"Создай список\" и запускайте меня, далее вам всё сразу станет понятным. \n \n" +
        "Если вы захотите поделиться списком с друзьями, нажмите кнопку \"Скопируй ссылку на список\" и отправьте эту ссылку в чате в телеграм. Все, кто перейдут по ней получат от меня ваш список и возможность его редактировать.\n \n" +
        "Исходный код проекта: \nhttps://github.com/WildWestWild/TelegramBot_Massive_Del";

    #endregion

    #region HistoryCommandMessages

    public static readonly string DontHaveListsInGetHistoryCommand =  "Нет истории использованных списков. Создавайте списки, либо переходите по ссылке и проссматривайте списки других.";
    
    #endregion

    #region NotFoundCommandMessages

    public static readonly string NotFoundMessageInNotFoundCommand =  "Извините, но это неизвестная для меня команда, пожалуйста, введите команду из меню, либо с виртуальной клавиатуры.";
    
    #endregion

    #region ReadCommandMessages

    public static readonly string NotFoundMessageInReadCommand = "Данный список не найден!";
    
    public static readonly string ItCannotShowInReadCommand = "Данный список не может быть отображен, так как в нём нет элементов. Попробуйте их добавить через виртуальную клавиатуру бота!";

    #endregion

    #region StartCommandMessages

    public static readonly string StartCommandMessage = "Давайте начнём!";

    #endregion

    #region StrikioutCommandMessages

    public static readonly string EnterElemInStrikingOutCommand = "Введите номер элемента: \n ! Чтобы выйти из режима записи в список, нажмите 'Завершить действие' ! \n ! Если хотите отменить вычеркивание, используйте это действия на вычеркнутый элемент !";

    public static readonly string WrongNumberInStrikingOutCommand = "Некорректный номер элемента! ";
    
    public static readonly string StrikingNumberInStrikingOutCommand = "Элемент вычеркнут!";
    
    public static readonly string ErrorInStrikingOutCommand = "Ошибка! Элемент не был вычеркнут.";

    #endregion

    #region UpdateCommandMessages

    public static readonly string ManySymbolsInUpdateCommand = "Невозможно добавить элемент, слишком много символов в списке!";
    
    public static readonly string InputElemInUpdateCommand = "Введите номер элемента и текст в формате - Номер элемента, пробел, текст. Например: (3 Привет,Мир!)";

    public static readonly string IncorrectNumberInUpdateCommand = "Элемент изменён!";
    
    public static readonly string SuccessInUpdateCommand = "Элемент изменён!";
    
    public static readonly string ErrorInUpdateCommand = "Ошибка! Элемент не был изменён.";

    #endregion
}