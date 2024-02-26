using Core.ListActions.ActionCommands;
using Core.ListActions.Actions;

namespace Infrastructure.TelegramBot.Validators;

public class CommandValidator
{
    private readonly ReadListAction _readListAction;

    public CommandValidator(ReadListAction readListAction)
    {
        _readListAction = readListAction;
    }
    
    public async Task<bool> CheckValidNumber(string message, ICommandIdentificator commandIdentificator, CancellationToken token)
    {
        try
        {
            var numberOfElement = Convert.ToUInt16(message);
            return numberOfElement < 1 && numberOfElement > await _readListAction.GetCountElements(commandIdentificator, token);
        }
        catch
        {
            return true;
        }
    }
}