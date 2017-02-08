using System.Collections.Generic;

namespace YouTown
{
    public interface IPlayerList : IList<IPlayer>
    {
        IPlayer Next(IPlayer current);
    }

    public class PlayerList : List<IPlayer>, IPlayerList
    {
        public PlayerList(IEnumerable<IPlayer> collection) : base(collection)
        {
        }

        public IPlayer Next(IPlayer current)
        {
            var index = IndexOf(current) + 1;
            if (index == Count)
            {
                index = 0;
            }
            return this[index];
        }

    }
}
