using System.Collections.Generic;
using System.Linq;

namespace YouTown
{
    public interface IDevelopmentCard : IGameItem
    {
        bool MaxOnePerTurn { get; }
        bool WaitOneTurnBeforePlay { get; }
        bool MoveToStockAfterPlay { get; }
//        bool turnAllowed(TurnPhase turn);
//        bool gameAllowed(GamePhase phase);
        ITurn TurnBought { get; set; }
        ITurn TurnPlayed { get; set; }
        IPlayer Player { get; set; }
        void Play(IGame game);
    }

    public abstract class DevelopmentCardBase
    {
        public int Id { get; protected set; }
        public virtual bool MaxOnePerTurn { get; }
        public virtual bool WaitOneTurnBeforePlay { get; }
        public virtual bool MoveToStockAfterPlay { get; }
        public ITurn TurnBought { get; set; }
        public ITurn TurnPlayed { get; set; }
        public IPlayer Player { get; set; }
    }

    public class DevelopmentCardCost : ResourceList
    {
        public DevelopmentCardCost()
        {
            AddRangeSafe(new Wheat(), new Sheep(), new Ore());
        }
    }

    public class VictoryPointCard : DevelopmentCardBase, IDevelopmentCard, IVictoryPoint
    {
        public VictoryPointCard(int id = Identifier.DontCare)
        {
            Id = id;
        }

        public override bool MoveToStockAfterPlay => true;
        public void Play(IGame game)
        {
            Player.VictoryPoints.Add(this);
        }

        public int VictoryPoints => 1;
    }

    public class Soldier : DevelopmentCardBase, IDevelopmentCard, IObscurable
    {
        public Soldier(int id = Identifier.DontCare, bool isAtServer = false, IPlayer playerAtClient = null)
        {
            PlayerAtClient = playerAtClient;
            IsAtServer = isAtServer;
            Id = id;
        }

        public override bool MaxOnePerTurn => true;
        public override bool WaitOneTurnBeforePlay => true;
        public override bool MoveToStockAfterPlay => true;
        public void Play(IGame game)
        {
            // TODO: implement
        }

        public bool IsAtServer { get; }
        public IPlayer PlayerAtClient { get; }
    }

    public class Invention : DevelopmentCardBase, IDevelopmentCard, IObscurable
    {
        public Invention(bool isAtServer = false, IPlayer playerAtClient = null)
        {
            PlayerAtClient = playerAtClient;
            IsAtServer = isAtServer;
        }

        public override bool MaxOnePerTurn => true;
        public override bool WaitOneTurnBeforePlay => true;
        public IResourceList PickedResources { get; set; }
        public bool IsAtServer { get; }
        public IPlayer PlayerAtClient { get; }

        public void Play(IGame game)
        {
            Player.GainResourcesFrom(game.Bank.Resources, PickedResources, this);
        }

    }

    public class Monopoly : DevelopmentCardBase, IDevelopmentCard, IObscurable
    {
        private readonly Dictionary<IPlayer, IResourceList> _stolen = 
            new Dictionary<IPlayer, IResourceList>();

        public Monopoly(bool isAtServer = false, IPlayer playerAtClient = null)
        {
            IsAtServer = isAtServer;
            PlayerAtClient = playerAtClient;
        }

        public override bool MaxOnePerTurn => true;
        public override bool WaitOneTurnBeforePlay => true;
        public ResourceType ResourceType { get; set; }
        public bool IsAtServer { get; }
        public IPlayer PlayerAtClient { get; }

        public IReadOnlyDictionary<IPlayer, IResourceList> Stolen => _stolen;

        public void Play(IGame game)
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
        }

        public override bool MaxOnePerTurn => true;
        public override bool WaitOneTurnBeforePlay => true;

        public void Play(IGame game)
        {
            var token1 = new Token(Player, game.Identifier.NewId());
            var token2 = new Token(Player, game.Identifier.NewId());
            token1.AddToPlayer(Player);
            token2.AddToPlayer(Player);
        }
    }
}
