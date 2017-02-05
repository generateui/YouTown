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
//        void Play(IGame game);
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

    public class VictoryPointCard : DevelopmentCardBase, IDevelopmentCard, IVictoryPoint
    {
        public VictoryPointCard(int id = Identifier.DontCare)
        {
            Id = id;
        }

        public override bool MoveToStockAfterPlay => true;
        public int VictoryPoints => 1;
    }

    public class Soldier : DevelopmentCardBase, IDevelopmentCard
    {
        public Soldier(int id = Identifier.DontCare)
        {
            Id = id;
        }

        public override bool MaxOnePerTurn => true;
        public override bool WaitOneTurnBeforePlay => true;
        public override bool MoveToStockAfterPlay => true;
    }

    public class Invention : DevelopmentCardBase, IDevelopmentCard
    {
        public override bool MaxOnePerTurn => true;
        public override bool WaitOneTurnBeforePlay => true;
        public IResourceList PickedResources { get; set; }
    }

    public class Monopoly : DevelopmentCardBase, IDevelopmentCard
    {
        public override bool MaxOnePerTurn => true;
        public override bool WaitOneTurnBeforePlay => true;
        public ResourceType ResourceType { get; set; }
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
    }
}
