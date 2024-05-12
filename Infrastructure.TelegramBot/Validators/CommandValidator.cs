using Core.ListActions.ActionCommands;
using Core.ListActions.Actions;

namespace Infrastructure.TelegramBot.Validators;

public class CommandValidator
{
    private readonly ReadListAction _readListAction;
    private const ushort MAX_COUNT_SYMBOLS = 15000;

    public CommandValidator(ReadListAction readListAction)
    {
        _readListAction = readListAction;
    }

    public async Task<bool> CheckInvalidNumber(string message, ICommandIdentificator commandIdentificator, CancellationToken token)
    {
        var numberOfElement = Convert.ToUInt16(message);
        return numberOfElement < 1 &&
               numberOfElement > await _readListAction.GetCountElements(commandIdentificator, token);
    }

    public async Task<bool> CheckMaxCountSymbolsInList(string message, ICommandIdentificator commandIdentificator, CancellationToken token)
    {
        var countSymbolsInMessage = message.Length;
        return countSymbolsInMessage + await _readListAction.GetCountSymbolsInList(commandIdentificator, token) > MAX_COUNT_SYMBOLS;
    }
}