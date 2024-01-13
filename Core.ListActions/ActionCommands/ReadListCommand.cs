namespace Core.ListActions.ActionCommands;

public struct ReadListCommand: ICommandIdentificator
{
    public long ChatId { get; set; }

    public string Name { get; set; }
}