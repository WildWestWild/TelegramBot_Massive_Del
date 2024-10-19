namespace Core.ListActions.ActionCommands;

public struct DeleteElementCommand: ICommandIdentificator
{
    public long ChatId { get; set; }
    public string Name { get; set; }
    public ushort[]? Numbers { get; set; }
}