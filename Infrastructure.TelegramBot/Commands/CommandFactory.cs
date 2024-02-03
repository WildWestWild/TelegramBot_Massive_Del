using Infrastructure.Storage.Models;
using Infrastructure.TelegramBot.Enums;

namespace Infrastructure.TelegramBot.Commands;

public class CommandFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ContextManager _contextManager;

    public CommandFactory(IServiceProvider serviceProvider, ContextManager contextManager)
    {
        _serviceProvider = serviceProvider;
        _contextManager = contextManager;
    }
    
    public async ValueTask<BaseCommand> CreateCommand(string commandName, long charId, CancellationToken token)
    {
        var commandType = commandName.ToCommandType();
        if (CreateCommandWithoutContext(commandType, out var command))
        {
            return command;
        }

        var userContext = await _contextManager.GetContext(charId, token);

        if (userContext is not null && CreateReadListCommand(commandName, userContext, out var readListCommand))
        {
            return readListCommand;
        }

        if (CreateCommandWithContext(commandType, userContext, out BaseCommand commandWithoutContext))
        {
            return commandWithoutContext;
        }

        return CreateNotFoundCommand();
    }

    private bool CreateCommandWithoutContext(CommandType? enumCommand, out BaseCommand command)
    {
        var commandType = enumCommand switch
        {
            CommandType.GetDescription => typeof(DescriptionCommand),
            _ => null
        };

        if (commandType is null)
        {
            command = default!;
            return false;
        }

        command = GetCommand(commandType);
        return true;
    }

    private bool CreateCommandWithContext(CommandType? enumCommand, UserContext? context, out BaseCommand command)
    {
        var commandType = enumCommand switch
        {
            CommandType.GetDescription => typeof(DescriptionCommand),
            _ => null
        };

        if (commandType is null)
        {
            command = default!;
            return false;
        }

        BaseCommandWithContext commandWithContext = GetCommand(commandType) as BaseCommandWithContext ?? throw new InvalidCastException();
        commandWithContext.SetContext(context);
        command = commandWithContext;
        return true;
    }
    
    private BaseCommand GetCommand(Type commandType)
    {
        return _serviceProvider.GetService(commandType) as BaseCommand 
               ?? throw new NullReferenceException(nameof(commandType));
    }

    private bool CreateReadListCommand(string commandName, UserContext context, out BaseCommand command)
    {
        throw new NotImplementedException();
    }

    private BaseCommand CreateNotFoundCommand() => GetCommand(typeof(NotFoundCommand));
}