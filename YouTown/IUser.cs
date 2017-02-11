namespace YouTown
{
    public interface IUser : IGameItem
    {
        string Name { get; } // TODO: introduce microtype
        PlayerColor Color { get; }
    }

    public class User : IUser
    {
        public User(int id, string name, PlayerColor color)
        {
            Id = id;
            Name = name;
            Color = color;
        }

        public User(UserData data)
        {
            Id = data.Id;
            Name = data.Name;
            Color = PlayerColor.Parse(data.Color);
        }

        public int Id { get; }
        public string Name { get; }
        public PlayerColor Color { get; }
    }
}
