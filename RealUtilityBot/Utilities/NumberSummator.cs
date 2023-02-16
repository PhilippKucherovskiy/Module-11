using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace RealUtilityBot.Utilities
{
    interface INumberSummator
    {
        public static async Task Sum(ITelegramBotClient botClient, long chatId, string messageText)
        {
            int sum = messageText.Split(' ').Select(int.Parse).Sum();
            await botClient.SendTextMessageAsync(chatId, $"Сумма чисел: {sum}");
        }
    }
}
