using System;

namespace YouTown
{
    public class Chat
    {
        public Chat(IPlayer player, IUser user, string text, DateTime dateTime)
        {
            Player = player;
            User = user;
            Text = text;
            DateTime = dateTime;
        }

        public IPlayer Player { get; }
        public IUser User { get; }
        public string Text { get; }
        public DateTime DateTime { get; }
    }
}
