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

    public class GameOptions : IGameOptions
    {
        public int VictoryPointsToWin { get; set; }
    }

    public class Game : IGame
    {
        private readonly List<IGamePhase> _gamePhases;

        public Game(IBoard board, IBank bank, IPlayerList players)
        {
            Board = board;
            Bank = bank;
            Players = players;
            GamePhase = DetermineFirstPlayer;
            _gamePhases = new List<IGamePhase>
            {
                DetermineFirstPlayer,
                SetupGamePhase,
                PlaceInitialPieces,
                PlayTurns,
                EndOfGame
            };
        }

        public IBoard Board { get; }
        public IBank Bank { get; }
        public IPlayerList Players { get; }
        public IGameOptions Options { get; } = new GameOptions();
        public IList<Chat> Chats { get; } = new List<Chat>();
        public IActionQueue Queue { get; } = new ActionQueue();
        public IIdentifier Identifier { get; } = new Identifier();
        public IGamePhase GamePhase { get; private set; }
        public DetermineFirstPlayer DetermineFirstPlayer { get; } = new DetermineFirstPlayer();
        public SetupGamePhase SetupGamePhase { get; } = new SetupGamePhase();
        public PlaceInitialPieces PlaceInitialPieces { get; } = new PlaceInitialPieces();
        public PlayTurns PlayTurns { get; } = new PlayTurns();
        public EndOfGame EndOfGame { get; } = new EndOfGame();

        public void MoveToNextPhase()
        {
            var index = _gamePhases.IndexOf(GamePhase);
            if (index - 1 >= _gamePhases.Count)
            {
                return;
            }
            var newIndex = index + 1;
            GamePhase.End(this);
            GamePhase = _gamePhases[newIndex];
            GamePhase.Start(this);
        }
    }
}
