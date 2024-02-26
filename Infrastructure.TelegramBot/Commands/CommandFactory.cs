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
        
        var commandType = commandName.ToCommandType() ?? GetCommandTypeByContext(userContext);
        if (CreateCommand(commandType, userContext, out BaseCommand command))
        {
            if (command.IsNeedSetEnterCommandText) 
                command.SetEnterCommandText(commandName);
            
            return command;
        }

        return CreateNotFoundCommand();
    }

    private bool CreateCommand(CommandType? enumCommand, UserContext? context, out BaseCommand command)
    {
        var commandType = enumCommand switch
        {
            CommandType.GetDescription => typeof(DescriptionCommand),
            CommandType.Start => typeof(StartCommand),
            CommandType.CreateNewList => typeof(CreateListCommand),
            CommandType.AddElement => typeof(AddCommand),
            CommandType.UpdateElement => typeof(UpdateCommand),
            CommandType.DeleteElement => typeof(DeleteCommand),
            _ => null
        };

        if (commandType is null)
        {
            command = default!;
            return false;
        }

        command = GetCommand(commandType);
        command.SetContext(context);
        return true;
    }
    
    private BaseCommand GetCommand(Type commandType)
    {
        return _serviceProvider.GetService(commandType) as BaseCommand 
               ?? throw new NullReferenceException(nameof(commandType));
    }

    private bool CreateReadListCommand(string commandName, UserContext context, out BaseCommand command)
    {
        command = default;
        return false;
    }

    private BaseCommand CreateNotFoundCommand() => GetCommand(typeof(NotFoundCommand));

    private CommandType? GetCommandTypeByContext(UserContext? context)
    {
        if (context?.Command is null)
            return null;

        return (CommandType) context.Command;
    }
}