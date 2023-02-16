using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using RealUtilityBot.Utilities;

namespace RealUtilityBot
{
    enum BotMode
    {
        NormalMode,
        SymbolCountMode,
        SumCalculationMode
    }
    class Bot : BackgroundService,INumberSummator, ICharacterCounter
    {
        private readonly ITelegramBotClient _telegramClient;
        private BotMode _currentMode = BotMode.NormalMode;

        public Bot(ITelegramBotClient telegramClient)
        {
            _telegramClient = telegramClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _telegramClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                new ReceiverOptions() { AllowedUpdates = { } }, // receive all update types
                cancellationToken: stoppingToken);

            Console.WriteLine("Bot started");
        }
        
        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update?.Type != UpdateType.Message || update.Message == null)
                return;

            if (_currentMode == BotMode.SymbolCountMode)
            {
                await INumberSummator.Sum(update.Message);
                return;
            }

            if (_currentMode == BotMode.SumCalculationMode)
            {
                await ICharacterCounter.Count(update.Message);
                return;
            }

            var message = update.Message;
            var chatId = message.Chat.Id;

            if (message.Text == "/start")
            {
                var replyKeyboard = new ReplyKeyboardMarkup(new[] {
                new KeyboardButton("Подсчет символов"),
                new KeyboardButton("Вычисление суммы")
            });
                var welcomeMessage = "Добро пожаловать! Я умею считать количество символов в сообщении или вычислять сумму чисел. Выберите, что хотите сделать, нажав на одну из кнопок в меню:";
                await _telegramClient.SendTextMessageAsync(chatId, welcomeMessage, replyMarkup: replyKeyboard, cancellationToken: cancellationToken);
                return;
            }

            if (message.Text == "/menu")
            {
                var replyKeyboard = new ReplyKeyboardMarkup(new[] {
                new KeyboardButton("Подсчет символов"),
                new KeyboardButton("Случайное слово"),});
            }
        }
        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiException => $"Telegram API Error:\n[{apiException.ErrorCode}]\n{apiException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);

            return Task.CompletedTask;
        }
        

    }
}
