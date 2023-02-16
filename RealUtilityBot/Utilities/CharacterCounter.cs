using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace RealUtilityBot.Utilities
{

    interface ICharacterCounter
    {
        public static async Task Count(ITelegramBotClient botClient, long chatId, string messageText)
        {
            int count = messageText.Length;
            await botClient.SendTextMessageAsync(chatId, $"В вашем сообщении {count} символов");
        }
    }


}
