using System.Collections.Generic;

namespace YouTown
{
    public interface IGame
    {
        IBoard Board { get; }
        IGameOptions Options { get; }
        IList<Chat> Chats { get; }
        IActionQueue Queue { get; }

        IBank Bank { get; }
        IPlayerList Players { get; }
        IIdentifier Identifier { get; }
        
        IGamePhase GamePhase { get; }
        DetermineFirstPlayer DetermineFirstPlayer { get; }
        SetupGamePhase SetupGamePhase { get; }
        PlaceInitialPieces PlaceInitialPieces { get; }
        PlayTurns PlayTurns { get; }
        EndOfGame EndOfGame { get; }
        void MoveToNextPhase();
    }

    public interface IGameOptions
    {
        int VictoryPointsToWin { get; set; }
    }
}
