using System.Collections.Generic;

namespace YouTown
{
    public interface IPlayerList : IList<IPlayer>
    {
        IPlayer Next(IPlayer current);
    }
}
