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
        

        var userContext = await _contextManager.GetContext(charId, token);

        if (userContext is not null && CreateReadListCommand(commandName, userContext, out var readListCommand))
        {
            return readListCommand;
        }
        
        var commandType = commandName.ToCommandType();
        if (CreateCommand(commandType, userContext, out BaseCommand commandWithoutContext))
        {
            return commandWithoutContext;
        }

        return CreateNotFoundCommand();
    }

    private bool CreateCommand(CommandType? enumCommand, UserContext? context, out BaseCommand command)
    {
        var commandType = enumCommand switch
        {
            CommandType.GetDescription => typeof(DescriptionCommand),
            CommandType.Start => typeof(StartCommand),
            _ => null
        };

        if (commandType is null)
        {
            command = default!;
            return false;
        }

        BaseCommand commandWithContext = GetCommand(commandType);
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