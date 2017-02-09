using System.Collections.Generic;

namespace YouTown
{
    public class Town : IPiece, IVertexPiece, IVictoryPoint, IProducer
    {
        public class TownCost : ResourceList
        {
            public TownCost()
            {
                AddRangeSafe(new Timber(), new Wheat(), new Sheep(), new Clay());
            }
        }
        public static readonly PieceType TownType = new PieceType("town");
        public Town(IPlayer player, int id = Identifier.DontCare, Vertex vertex = null)
        {
            Player = player;
            Id = id;
            Vertex = vertex;
        }

        public IPlayer Player { get; }
        public int Id { get; }
        public Vertex Vertex { get; set; }
        public PieceType PieceType => TownType;
        public bool AffectsRoad => true;
        public IResourceList Cost => new TownCost();
        public int VictoryPoints => 1;

        public bool IsAt(Location location) => Vertex.Locations.Contains(location);

        public IResourceList Produce(IHex hex)
        {
            return new ResourceList(hex.Produce());
        }

        public void AddToPlayer(IPlayer player)
        {
            player.Towns[Vertex] = this;
            player.Pieces.Add(this);
            player.Stock[TownType].Remove(this);
            player.VictoryPoints.Add(this);
            if (!player.VertexPieces.ContainsKey(Vertex))
            {
                player.VertexPieces[Vertex] = new List<IVertexPiece>();
            }
            player.VertexPieces[Vertex].Add(this);
            player.Producers.Add(this);
        }

        public void RemoveFromPlayer(IPlayer player)
        {
            player.Towns.Remove(Vertex);
            player.Pieces.Remove(this);
            player.Stock[TownType].Add(this);
            player.VictoryPoints.Remove(this);
            player.VertexPieces[Vertex].Remove(this);
            player.Producers.Remove(this);
        }

        public void AddToBoard(IBoard board)
        {
            board.TownsByVertex[Vertex] = this;
            board.PiecesByVertex[Vertex] = this;
            board.Pieces.Add(this);
            board.Producers.Add(this);
        }

        public void RemoveFromBoard(IBoard board)
        {
            board.TownsByVertex.Remove(Vertex);
            board.PiecesByVertex.Remove(Vertex);
            board.Pieces.Remove(this);
            board.Producers.Remove(this);
        }

    }
}