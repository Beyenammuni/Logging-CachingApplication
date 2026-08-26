using Logging_CachingApplication.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace Logging_CachingInfrastructure.Data
{
    public class TelegramService : ITelegramService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public TelegramService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task SendMessageAsync(string message, CancellationToken cancellationToken)
        {
            var botToken = _configuration["Telegram:BotToken"];
            var chatId = _configuration["Telegram:ChatId"];

            if (string.IsNullOrEmpty(botToken) || string.IsNullOrEmpty(chatId))
            {
                throw new InvalidOperationException("Telegram BotToken or ChatId is not configured (check appsettings or environment).");
            }

            var url = $"https://api.telegram.org/bot{botToken}/sendMessage";

            var response = await _httpClient.PostAsJsonAsync(
                url,
                new { chat_id = chatId, text = message },
                cancellationToken);

            response.EnsureSuccessStatusCode();
        }
    }
}
