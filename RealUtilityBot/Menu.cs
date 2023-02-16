using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using RealUtilityBot.Utilities;

namespace RealUtilityBot
{
    public class Menu
    {
        public static async Task Send(ITelegramBotClient botClient, long chatId)
        {
            var replyMarkup = new ReplyKeyboardMarkup(new[]
            {
                new[]
                {
                    new KeyboardButton("Подсчет символов"),
                    new KeyboardButton("Вычисление суммы чисел")
                }
            })
            {
                ResizeKeyboard = true
            };

            await botClient.SendTextMessageAsync(chatId, "Главное меню", replyMarkup: replyMarkup);
        }
    }
}
