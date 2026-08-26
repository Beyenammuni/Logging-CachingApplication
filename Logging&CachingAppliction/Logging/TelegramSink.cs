using Serilog.Core;
using Serilog.Events;
using System.Net.Http.Json;

namespace Logging_CachingApi.Logging;

public class TelegramSink : ILogEventSink
{
    private readonly string _botToken;
    private readonly string _chatId;
    private readonly HttpClient _httpClient;

    public TelegramSink(
        string botToken,
        string chatId)
    {
        _botToken = botToken;
        _chatId = chatId;

        _httpClient = new HttpClient();
    }

    public void Emit(LogEvent logEvent)
    {
        if (logEvent.Level < LogEventLevel.Error)
            return;

        var message =
            $"🚨 {logEvent.Level}\n\n" +
            $"{logEvent.RenderMessage()}";

        if (logEvent.Exception != null)
        {
            message +=
                $"\n\nException:\n{logEvent.Exception.Message}";
        }

        _ = SendToTelegramAsync(message);
    }

    private async Task SendToTelegramAsync(string message)
    {
        try
        {
            var url =
                $"https://api.telegram.org/bot{_botToken}/sendMessage";

            await _httpClient.PostAsJsonAsync(
                url,
                new
                {
                    chat_id = _chatId,
                    text = message
                });
        }
        catch
        {
            // مهم:
            // لا نعمل Logging هنا حتى لا يحدث Loop
        }
    }
}