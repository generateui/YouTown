using System.Collections.Generic;

namespace YouTown
{
    public interface IServerGame
    {
        IList<IDevelopmentCard> DevelopmentCards { get; }
        IDictionary<IPlayer, IList<IDevelopmentCard>> DevelopmentCardsByPlayer { get; }
    }
}