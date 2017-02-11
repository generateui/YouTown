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

        public Chat(ChatData data, IRepository repo)
        {
            User = repo.Get<IUser>(data.UserId);
            Player = repo.Get<IPlayer>(data.PlayerId);
            Text = data.Text;
            DateTime = new DateTime(data.DateTime);
        }

        public IPlayer Player { get; }
        public IUser User { get; }
        public string Text { get; }
        public DateTime DateTime { get; }

        public ChatData ToData() =>
            new ChatData
            {
                PlayerId = Player.Id,
                UserId = User.Id,
                DateTime = DateTime.Ticks,
                Text = Text
            };
    }
}
