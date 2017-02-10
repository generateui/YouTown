using System.Collections.Generic;

namespace YouTown
{
    public interface IServerGame
    {
        IList<IDevelopmentCard> DevelopmentCards { get; }
        IDictionary<IPlayer, IList<IDevelopmentCard>> DevelopmentCardsByPlayer { get; }
        IRandom Random { get; }
        IGame Game { get; }
        IIdentifier Identifier { get; }
        IList<IUser> Users { get; }
        IDictionary<IUser, IPlayer> PlayersByUser { get; }
        ISetupOptions SetupOptions { get; }

        IPlayTurnsTurn GetNextTurn();
//        {
//            var number = Turn.Number + 1;
//            var player = game.Players.Next(Turn.Player);
//            return new PlayTurnsTurn(number, player);
//        }
    }
}