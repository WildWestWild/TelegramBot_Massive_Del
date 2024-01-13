namespace Core.ListActions.ActionCommands;

public struct AddListCommand: ICommandIdentificator
{
    public long ChatId { get; set; }
    public string Name { get; set; }
}