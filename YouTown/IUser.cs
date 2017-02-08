using System;

namespace YouTown
{
    public interface IUser
    {
        Guid Id { get; }
        string Name { get; } // TODO: introduce microtype
        PlayerColor Color { get; }
    }

    public class User : IUser
    {
        public User(Guid id, string name, PlayerColor color)
        {
            Id = id;
            Name = name;
            Color = color;
        }

        public Guid Id { get; }
        public string Name { get; }
        public PlayerColor Color { get; }
    }
}
