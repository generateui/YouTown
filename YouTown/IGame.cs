using System.Collections.Generic;

namespace YouTown
{
    public interface IGame
    {
        IPlayOptions PlayOptions { get; }
        ISetupOptions SetupOptions { get; }
        IList<Chat> Chats { get; }
        IActionQueue Queue { get; }

        IBoard Board { get; }
        IBank Bank { get; }
        IPlayerList Players { get; }
        LargestArmy LargestArmy { get; set; }
        // LongestRoad LongestRoad { get; }
        
        IGamePhase GamePhase { get; }
        DetermineFirstPlayer DetermineFirstPlayer { get; }
        SetupGamePhase SetupGamePhase { get; }
        PlaceInitialPieces PlaceInitialPieces { get; }
        PlayTurns PlayTurns { get; }
        EndOfGame EndOfGame { get; }
        void MoveToNextPhase();
    }

    public interface ISetupOptions
    {
        string Name { get; }
        IBoard Board { get; }
        int RoadCount { get; }
        int TownCount { get; }
        int CityCount { get; }
        IPortList Ports { get; }
        IHexList Hexes { get; }
        IChitList Chits { get; }
        IDictionary<ResourceType, int> ResourceCountByType { get; }
        IDictionary<DevelopmentCardType, int> DevelopmentCardCountByType { get; }
    }

    public class SetupOptions : ISetupOptions
    {
        public string Name { get; set; }
        public IBoard Board { get; set; }
        public int RoadCount { get; set; }
        public int TownCount { get; set; }
        public int CityCount { get; set; }
        public IPortList Ports { get; set; }
        public IHexList Hexes { get; set; }
        public IChitList Chits { get; set; }
        public IDictionary<ResourceType, int> ResourceCountByType { get; set; }
        public IDictionary<DevelopmentCardType, int> DevelopmentCardCountByType { get; set; }
    }

    public interface IPlayOptions
    {
        int VictoryPointsToWin { get; set; }
    }

    public class PlayOptions : IPlayOptions
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
        public LargestArmy LargestArmy { get; set; }
        public IPlayOptions PlayOptions { get; } = new PlayOptions();
        public ISetupOptions SetupOptions { get; }
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
