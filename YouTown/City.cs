namespace YouTown
{
    public class City : IPiece, IPointPiece, IVictoryPoint
    {
        public class CityCost : ResourceList
        {
            public CityCost()
            {
                AddRangeSafe(new Wheat(), new Wheat(), new Ore(), new Ore(), new Ore());
            }
        }
        public static readonly PieceType CityType = new PieceType("city");

        public City(IPlayer player, int id = Identifier.DontCare, Point point = null)
        {
            Player = player;
            Id = id;
            Point = point;
        }

        public IPlayer Player { get; }
        public int Id { get; }
        public Point Point { get; }
        public PieceType PieceType => CityType;
        public bool AffectsRoad => false;
        public IResourceList Cost => new CityCost();
        public int VictoryPoints => 2;

        public void AddToPlayer(IPlayer player)
        {
            player.Cities[Point] = this;
            player.Pieces.Add(this);
            player.Stock[CityType].Remove(this);
            player.VictoryPoints.Add(this);
            player.PointPieces[Point].Add(this);
            // TODO: IProducer
        }

        public void RemoveFromPlayer(IPlayer player)
        {
            player.Cities.Remove(Point);
            player.Pieces.Remove(this);
            player.Stock[CityType].Add(this);
            player.VictoryPoints.Remove(this);
            player.PointPieces[Point].Remove(this);
            // TODO: IProducer
        }

        public void AddToBoard(IBoard board)
        {
            board.CitiesByPoint[Point] = this;
            board.PiecesByPoint[Point] = this;
            board.Pieces.Add(this);
        }

        public void RemoveFromBoard(IBoard board)
        {
            board.CitiesByPoint.Remove(Point);
            board.PiecesByPoint.Remove(Point);
            board.Pieces.Remove(this);
        }
    }
}