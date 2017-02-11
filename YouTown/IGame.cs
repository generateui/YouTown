using System.Collections.Generic;
using System.Linq;
using YouTown.GameAction;

namespace YouTown
{
    public interface IGame
    {
        IPlayOptions PlayOptions { get; }
        ISetupOptions SetupOptions { get; }
        IList<Chat> Chats { get; }
        IActionQueue Queue { get; }
        IRepository Repository { get; }
        IList<IGameAction> Actions { get; }
            
        IBoardForPlay Board { get; }
        IBank Bank { get; }
        IList<IUser> Users { get; }
        IPlayerList Players { get; }
        LargestArmy LargestArmy { get; set; }
        // LongestRoad LongestRoad { get; }
        
        IGamePhase GamePhase { get; }
        DetermineFirstPlayer DetermineFirstPlayer { get; }
        SetupGamePhase SetupGamePhase { get; }
        PlaceInitialPieces PlaceInitialPieces { get; }
        PlayTurns PlayTurns { get; }
        EndOfGame EndOfGame { get; }
        IIdentifier Identifier { get; }
        void MoveToNextPhase();
    }

    public interface ISetupOptions
    {
        string Name { get; }
        IBoardDesign BoardDesign { get; }
        int RoadCount { get; }
        int TownCount { get; }
        int CityCount { get; }
        IList<IPort> Ports { get; }
        IList<IHex> Hexes { get; }
        IList<IChit> Chits { get; }
        IDictionary<ResourceType, int> ResourceCountByType { get; }
        IDictionary<DevelopmentCardType, int> DevelopmentCardCountByType { get; }
    }

    public class SetupOptions : ISetupOptions
    {
        public string Name { get; set; }
        public IBoardDesign BoardDesign { get; set; }
        public int RoadCount { get; set; }
        public int TownCount { get; set; }
        public int CityCount { get; set; }
        public IList<IPort> Ports { get; set; }
        public IList<IHex> Hexes { get; set; }
        public IList<IChit> Chits { get; set; }
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

        public Game(IBoardForPlay board, IBank bank, IPlayerList players, IPlayOptions playOptions)
        {
            Players = players;
            Repository.AddAll(Players.Select(p => p.User));
            Repository.AddAll(Players);
            Repository.AddAll(Players.SelectMany(p => p.Ports));
            Repository.AddAll(players.SelectMany(p => p.Stock.SelectMany(x => x.Value)));

            Board = board;
            Repository.AddAll(board.HexesByLocation.Values);
            Repository.AddAll(board.Ports);
            Repository.AddAll(board.Robber);
            Repository.AddAll(board.HexesByLocation.Values.Where(h => h.Chit != null).Select(h => h.Chit));

            Bank = bank;
            Repository.AddAll(bank.Resources);
            Repository.AddAll(bank.DevelopmentCards);

            LargestArmy = new LargestArmy(Identifier.NewId());
            Repository.Add(LargestArmy);

            PlayOptions = playOptions;

            DetermineFirstPlayer = new DetermineFirstPlayer(Identifier.NewId());
            SetupGamePhase = new SetupGamePhase(Identifier.NewId());
            PlaceInitialPieces = new PlaceInitialPieces(Identifier.NewId());
            PlayTurns = new PlayTurns(Identifier.NewId(), Repository, Identifier);
            EndOfGame = new EndOfGame(Identifier.NewId());
            Repository.AddAll(DetermineFirstPlayer, SetupGamePhase, PlaceInitialPieces, PlayTurns, EndOfGame);

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

        public Game(GameData data)
        {
            var repo = Repository;
            Users = data.Users.Select(u => new User(u)).Cast<IUser>().ToList();
            repo.AddAll(Users);

            Players = new PlayerList(data.Players.Select(pd => pd.FromData(repo)));
            repo.AddAll(Players);
            repo.AddAll(Players.SelectMany(p => p.Ports));
            repo.AddAll(Players.SelectMany(p => p.Ports));
            repo.AddAll(Players.SelectMany(p => p.Stock.SelectMany(x => x.Value)));

            Board = new BoardForPlay(data.Board, repo);
            repo.AddAll(Board.HexesByLocation.Values);
            repo.AddAll(Board.Ports);
            repo.Add(Board.Robber);
            Repository.AddAll(Board.HexesByLocation.Values.Where(h => h.Chit != null).Select(h => h.Chit));

            Bank = data.Bank.FromData(repo);
            repo.AddAll(Bank.Resources);
            repo.AddAll(Bank.DevelopmentCards);

            LargestArmy = new LargestArmy(data.LargestArmy, repo);
            repo.Add(LargestArmy);

            PlayOptions = data.PlayOptions.FromData();
            SetupOptions = data.SetupOptions.FromData();
            Chats = data.Chats.Select(c => new Chat(c, repo)).ToList();
            Queue = data.Queue.FromData(repo);

            Actions = data.Actions.Select(a => a.FromData(repo)).ToList();
            repo.AddAll(Actions);

            DetermineFirstPlayer = data.DetermineFirstPlayer.FromData();
            SetupGamePhase = data.Setup.FromData();
            PlaceInitialPieces = new PlaceInitialPieces(data.PlaceInitialPieces, repo);
            PlayTurns = new PlayTurns(data.PlayTurns, repo);
            EndOfGame = data.EndOfGame.FromData();
            repo.AddAll(DetermineFirstPlayer, SetupGamePhase, PlaceInitialPieces, PlayTurns, EndOfGame);

            GamePhase = repo.Get<IGamePhase>(data.GamePhaseId);
        }

        public IList<IGameAction> Actions { get; } = new List<IGameAction>();
        public IBoardForPlay Board { get; }
        public IBank Bank { get; }
        public IList<IUser> Users { get; } = new List<IUser>();
        public IPlayerList Players { get; }
        public LargestArmy LargestArmy { get; set; }
        public IPlayOptions PlayOptions { get; }
        public ISetupOptions SetupOptions { get; }
        public IList<Chat> Chats { get; } = new List<Chat>();
        public IActionQueue Queue { get; } = new ActionQueue();
        public IRepository Repository { get; } = new Repository();
        public IIdentifier Identifier { get; } = new Identifier();
        public IGamePhase GamePhase { get; private set; }
        public DetermineFirstPlayer DetermineFirstPlayer { get; }
        public SetupGamePhase SetupGamePhase { get; }
        public PlaceInitialPieces PlaceInitialPieces { get; }
        public PlayTurns PlayTurns { get; } 
        public EndOfGame EndOfGame { get; }

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
