using Infrastructure.Storage.Models;
using Infrastructure.TelegramBot.Commands;
using Infrastructure.TelegramBot.Enums;
using Infrastructure.TelegramBot.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.TelegramBot.BotManagers;

public class CommandFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ContextManager _contextManager;
    private const ushort MIN_SYMBOLS_FOR_UNIQUE_LIST = 40;

    public CommandFactory(IServiceProvider serviceProvider, ContextManager contextManager)
    {
        _serviceProvider = serviceProvider;
        _contextManager = contextManager;
    }
    
    public async ValueTask<BaseCommand> CreateCommand(string message, long charId, CancellationToken token)
    {
        if (CreateReadListCommand(message, out var readListCommand))
        {
            readListCommand.SetEnterCommandText(message);
            return readListCommand;
        }
        
        var userContext = await _contextManager.GetContext(charId, token);

        if (CancelCommand.IsNeedToUseCancelCommand(message))
        {
            return CreateCancelCommand(userContext: userContext);
        }
        
        var commandType = message.ToCommandType() ?? GetCommandTypeByContext(userContext);
        if (CreateCommand(commandType, userContext, out BaseCommand command))
        {
            if (command.IsNeedSetEnterCommandText) 
                command.SetEnterCommandText(message);
            
            return command;
        }

        return CreateNotFoundCommand();
    }

    public BaseCommand CreateCancelCommand(string? message = null, UserContext? userContext = null)
    {
        var cancelCommand = _serviceProvider.GetRequiredService<CancelCommand>();
        if (message is not null)
            cancelCommand.SetEnterCommandText(message);
        
        if (userContext is not null)
            cancelCommand.SetContext(userContext);
        
        return cancelCommand;
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
            CommandType.StrikingOutElement => typeof(StrikingOutCommand),
            CommandType.CopyLink => typeof(CopyLinkCommand),
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

    private bool CreateReadListCommand(string commandName, out BaseCommand command)
    {
        if (commandName.Length >= MIN_SYMBOLS_FOR_UNIQUE_LIST 
            && (ReadCommand.FindListInCommandText.IsMatch(commandName) || ReadCommand.FindGuidLinkInText.IsMatch(commandName)))
        {
            command = GetCommand(typeof(ReadCommand));
            return true;
        }
        
        command = default!;
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