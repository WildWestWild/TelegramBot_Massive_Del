namespace Core.ListActions.ActionCommands;

public interface ICommandIdentificator
{
    public long ChatId { get; }

    public string Name { get; }
}