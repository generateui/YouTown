using System.Collections.Generic;

namespace YouTown
{
    public interface IGame
    {
        IBank Bank { get; }
        IList<IPlayer> Players { get; }
        IIdentifier Identifier { get; }

        IGamePhase GamePhase { get; }
        DetermineFirstPlayer DetermineFirstPlayer { get; }
        SetupGamePhase SetupGamePhase { get; }
        PlaceInitialPieces PlaceInitialPieces { get; }
        PlayTurns PlayTurns { get; }
        EndOfGame EndOfGame { get; }
    }
}
