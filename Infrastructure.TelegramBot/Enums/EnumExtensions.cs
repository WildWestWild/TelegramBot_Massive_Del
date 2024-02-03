using System.ComponentModel;
using System.Reflection;

namespace Infrastructure.TelegramBot.Enums;

public static class EnumExtensions
{
    private static Dictionary<string, CommandType> _commandTypes;

    static EnumExtensions()
    {
        _commandTypes = new Dictionary<string, CommandType>();
        Type enumType = typeof(CommandType);
        var fields = enumType.GetFields();

        foreach (var field in fields)
        {
            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute descriptionAttribute)
            {
                var commandType = (CommandType)(field.GetValue(null) ?? throw new ArgumentException(nameof(CommandType)));
                _commandTypes.Add(descriptionAttribute.Description, commandType); 
            }
        }
    }

    public static CommandType? ToCommandType(this string source)
        => _commandTypes.TryGetValue(source, out CommandType commandType)
            ? commandType
            : null;
    
}