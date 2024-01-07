using Infrastructure.TelegramBot;
using Infrastructure.TelegramBot.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions();
builder.Services.Configure<BotOptions>(builder.Configuration.GetSection(nameof(BotOptions)));
builder.Services
    .AddControllers()
    .AddNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTelegramBot(
    builder.Configuration
        .GetSection(nameof(BotOptions))
        .Get<BotOptions>()
    ?? throw new ArgumentNullException(nameof(BotOptions))
    );

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
