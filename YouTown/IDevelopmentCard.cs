using System;
using System.Collections.Generic;
using System.Linq;

namespace YouTown
{
    public sealed class DevelopmentCardType
    {
        private readonly string _type;
        private readonly Func<int, IDevelopmentCard> _factory;

        public DevelopmentCardType(string type, Func<int, IDevelopmentCard> factory)
        {
            _type = type;
            _factory = factory;
        }

        public IDevelopmentCard Create(int id) => _factory(id);

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return string.Equals(_type, ((DevelopmentCardType)obj)._type);
        }

        /// <inheritdoc />
        public override int GetHashCode() => _type?.GetHashCode() ?? 0;

        /// <inheritdoc />
        public override string ToString() => _type;
    }

    public interface IDevelopmentCard : IPiece
    {
        new IPlayer Player { get; set; }
        DevelopmentCardType DevelopmentCardType { get; }
        bool MaxOnePerTurn { get; }
        bool WaitOneTurnBeforePlay { get; }
        bool MoveToStockAfterPlay { get; }
//        bool turnAllowed(TurnPhase turn);
//        bool gameAllowed(GamePhase phase);
//        void AddToBoard(IBoard board);
//        void RemoveFromBoard(IBoard board);
        IPlayTurnsTurn TurnBought { get; set; }
        IPlayTurnsTurn TurnPlayed { get; set; }
        void Play(IGame game);
        void PerformAtServer(IServerGame serverGame);
    }

    public class DevelopmentCardCost : ResourceList
    {
        public DevelopmentCardCost()
        {
            AddRangeSafe(new Wheat(), new Sheep(), new Ore());
        }
    }

    public abstract class DevelopmentCardBase : IDevelopmentCard
    {
        public static PieceType DevelopmentCardPieceType = new PieceType("developmentcard");
        public int Id { get; protected set; }
        public DevelopmentCardType DevelopmentCardType { get; }
        public virtual bool MaxOnePerTurn { get; }
        public virtual bool WaitOneTurnBeforePlay { get; }
        public virtual bool MoveToStockAfterPlay { get; }
        public IPlayTurnsTurn TurnBought { get; set; }
        public IPlayTurnsTurn TurnPlayed { get; set; }
        public IPlayer Player { get; set; }
        public PieceType PieceType => DevelopmentCardPieceType;
        public bool AffectsRoad => false;

        public IResourceList Cost => new DevelopmentCardCost();

        public virtual void Play(IGame game)
        {
            throw new System.NotImplementedException();
        }

        public virtual void PerformAtServer(IServerGame serverGame)
        {
        }

        public void AddToPlayer(IPlayer player)
        {
            player.DevelopmentCards.Add(this);
        }

        public void RemoveFromPlayer(IPlayer player)
        {
            player.DevelopmentCards.Remove(this);
        }

        public void AddToBoard(IBoard board)
        {
        }

        public void RemoveFromBoard(IBoard board)
        {
        }
    }

    public class DummyDevelopmentCard : DevelopmentCardBase
    {
        public static DevelopmentCardType VictoryPointCardType =
            new DevelopmentCardType("DummyDevelopmentCard", id => new DummyDevelopmentCard(id));

        public DummyDevelopmentCard(int id = Identifier.DontCare)
        {
            Id = id;
        }
    }

    public class VictoryPointCard : DevelopmentCardBase, IDevelopmentCard, IVictoryPoint
    {
        public static DevelopmentCardType VictoryPointCardType = 
            new DevelopmentCardType("VictoryPointCard", id => new VictoryPointCard(id));
        public VictoryPointCard(int id = Identifier.DontCare)
        {
            Id = id;
        }

        public override bool MoveToStockAfterPlay => true;
        public override void Play(IGame game)
        {
            Player.VictoryPoints.Add(this);
        }

        public int VictoryPoints => 1;
    }

    public class Soldier : DevelopmentCardBase, IDevelopmentCard, IObscurable
    {
        public static DevelopmentCardType SoldierType = 
            new DevelopmentCardType("Soldier", id => new Soldier(id));

        public Soldier(int id = Identifier.DontCare, bool isAtServer = false, IPlayer playerAtClient = null)
        {
            PlayerAtClient = playerAtClient;
            IsAtServer = isAtServer;
            Id = id;
        }

        public override bool MaxOnePerTurn => true;
        public override bool WaitOneTurnBeforePlay => true;
        public override bool MoveToStockAfterPlay => true;
        public override void Play(IGame game)
        {
            // TODO: implement
        }

        public bool IsAtServer { get; }
        public IPlayer PlayerAtClient { get; }
    }

    public class Invention : DevelopmentCardBase, IDevelopmentCard, IObscurable
    {
        public static DevelopmentCardType InventionType = 
            new DevelopmentCardType("Invention", id => new Invention(id));

        public Invention(int id = Identifier.DontCare, bool isAtServer = false, IPlayer playerAtClient = null)
        {
            Id = id;
            PlayerAtClient = playerAtClient;
            IsAtServer = isAtServer;
        }

        public override bool MaxOnePerTurn => true;
        public override bool WaitOneTurnBeforePlay => true;
        public IResourceList PickedResources { get; set; }
        public bool IsAtServer { get; }
        public IPlayer PlayerAtClient { get; }

        public override void Play(IGame game)
        {
            Player.GainResourcesFrom(game.Bank.Resources, PickedResources, this);
        }

    }

    public class Monopoly : DevelopmentCardBase, IDevelopmentCard, IObscurable
    {
        public static DevelopmentCardType MonopolyType = new DevelopmentCardType("Monopoly", id => new Monopoly(id));
        private readonly Dictionary<IPlayer, IResourceList> _stolen = 
            new Dictionary<IPlayer, IResourceList>();

        public Monopoly(int id = Identifier.DontCare, bool isAtServer = false, IPlayer playerAtClient = null)
        {
            Id = id;
            IsAtServer = isAtServer;
            PlayerAtClient = playerAtClient;
        }

        public override bool MaxOnePerTurn => true;
        public override bool WaitOneTurnBeforePlay => true;
        public ResourceType ResourceType { get; set; }
        public bool IsAtServer { get; }
        public IPlayer PlayerAtClient { get; }

        public IReadOnlyDictionary<IPlayer, IResourceList> Stolen => _stolen;

        public override void Play(IGame game)
        {
            var opponents = game.Players.Where(p => p != Player);
            foreach (var opponent in opponents)
            {
                IResourceList toSteal = opponent.Hand.OfType(ResourceType);
                _stolen[opponent] = toSteal;
                Player.GainResourcesFrom(opponent.Hand, toSteal, this);
            }
        }
    }

    /// <summary>
    /// When playing, it adds two tokens to a player's set of pieces. During
    /// the players' turn, when he builds a road he will automatically use
    /// a token if any available instead of paying for it by resources.
    /// </summary>
    public class RoadBuilding : DevelopmentCardBase, IDevelopmentCard
    {
        /// <summary>
        /// Though this can be solved with a simple counter, this is nicer
        /// because it's now easier to build visualization on top of this
        /// </summary>
        public class Token : IPiece
        {
            public static readonly PieceType RoadBuildingTokenType = new PieceType("roadbuildingtoken");
            public Token(IPlayer player, int id)
            {
                Id = id;
                Player = player;
            }

            public IPlayer Player { get; }
            public int Id { get; }
            public PieceType PieceType => RoadBuildingTokenType;
            public bool AffectsRoad => false;
            public IResourceList Cost => ResourceList.Empty;

            public void AddToPlayer(IPlayer player)
            {
                player.Pieces.Add(this);
            }

            public void RemoveFromPlayer(IPlayer player)
            {
                player.Pieces.Remove(this);
            }

            public void AddToBoard(IBoard board) { }

            public void RemoveFromBoard(IBoard board) { }
        }

        public static DevelopmentCardType RoadBuildingType = new DevelopmentCardType("RoadBuilding", id => new RoadBuilding(id));

        public RoadBuilding(int id = Identifier.DontCare)
        {
            Id = id;
        }

        public override bool MaxOnePerTurn => true;
        public override bool WaitOneTurnBeforePlay => true;
        public IEnumerable<Token> Tokens { get; private set; }

        public override void PerformAtServer(IServerGame serverGame)
        {
            var token1 = new Token(Player, serverGame.Identifier.NewId());
            var token2 = new Token(Player, serverGame.Identifier.NewId());
            Tokens = new List<Token> { token1, token2 };
        }

        public override void Play(IGame game)
        {
            foreach (var token in Tokens)
            {
                token.AddToPlayer(Player);
            }
        }
    }
}
