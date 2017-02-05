using System;

namespace YouTown
{
    public interface IUser
    {
        Guid Id { get; }
        string Name { get; } // TODO: introduce microtype
    }

    public class User : IUser
    {
        public User(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; }
        public string Name { get; }
    }
}
