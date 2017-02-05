namespace YouTown
{
    public class Road : IPiece, IEdgePiece
    {
        public static readonly PieceType RoadType = new PieceType("road");

        public class RoadCost : ResourceList
        {
            public RoadCost()
            {
                AddRangeSafe(new Timber(), new Clay());
            }
        }

        public Road(IPlayer player, int id = Identifier.DontCare, Edge edge = null)
        {
            Player = player;
            Id = id;
            Edge = edge;
        }

        public IPlayer Player { get; }
        public int Id { get; }
        public Edge Edge { get; }
        public PieceType PieceType => RoadType;
        public bool AffectsRoad => true;
        public IResourceList Cost => new RoadCost();

        public void AddToPlayer(IPlayer player)
        {
            player.Roads[Edge] = this;
            player.Pieces.Add(this);
            player.Stock[RoadType].Remove(this);
            player.EdgePieces[Edge].Add(this);
        }
        public void RemoveFromPlayer(IPlayer player)
        {
            player.Roads.Remove(Edge);
            player.Pieces.Remove(this);
            player.Stock[RoadType].Add(this);
            player.EdgePieces[Edge].Remove(this);
        }
    }
}