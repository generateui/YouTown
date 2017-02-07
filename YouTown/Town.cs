namespace YouTown
{
    public class Town : IPiece, IPointPiece, IVictoryPoint
    {
        public class TownCost : ResourceList
        {
            public TownCost()
            {
                AddRangeSafe(new Timber(), new Wheat(), new Sheep(), new Clay());
            }
        }
        public static readonly PieceType TownType = new PieceType("town");
        public Town(IPlayer player, int id = Identifier.DontCare, Point point = null)
        {
            Player = player;
            Id = id;
            Point = point;
        }

        public IPlayer Player { get; }
        public int Id { get; }
        public Point Point { get; set; }
        public PieceType PieceType => TownType;
        public bool AffectsRoad => true;
        public IResourceList Cost => new TownCost();
        public int VictoryPoints => 1;

        public void AddToPlayer(IPlayer player)
        {
            player.Towns[Point] = this;
            player.Pieces.Add(this);
            player.Stock[TownType].Remove(this);
            player.VictoryPoints.Add(this);
            player.PointPieces[Point].Add(this);
            // TODO: IProducer
        }

        public void RemoveFromPlayer(IPlayer player)
        {
            player.Towns.Remove(Point);
            player.Pieces.Remove(this);
            player.Stock[TownType].Add(this);
            player.VictoryPoints.Remove(this);
            player.PointPieces[Point].Remove(this);
            // TODO: IProducer
        }

        public void AddToBoard(IBoard board)
        {
            board.TownsByPoint[Point] = this;
            board.PiecesByPoint[Point] = this;
            board.Pieces.Add(this);
        }

        public void RemoveFromBoard(IBoard board)
        {
            board.TownsByPoint.Remove(Point);
            board.PiecesByPoint.Remove(Point);
            board.Pieces.Remove(this);
        }
    }
}