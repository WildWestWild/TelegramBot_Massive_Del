namespace Infrastructure.TelegramBot.Options;

public record BotOptions(string BotTokenFileName, string HostAddress, string Route, string SecretTokenFileName);