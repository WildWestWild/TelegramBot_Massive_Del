namespace Core.ListActions.ActionCommands;

public struct AddOrUpdateElementCommand: ICommandIdentificator
{
    public long ChatId { get; set; }
    public string Name { get; set; }

    public ushort Number { get; set; }

    public string Data { get; set; }
}