using System.Collections.Generic;

namespace YouTown
{
    public class City : IPiece, IVertexPiece, IVictoryPoint, IProducer
    {
        public class CityCost : ResourceList
        {
            public CityCost()
            {
                AddRangeSafe(new Wheat(), new Wheat(), new Ore(), new Ore(), new Ore());
            }
        }
        public static readonly PieceType CityType = new PieceType("city");

        public City(IPlayer player, int id = Identifier.DontCare, Vertex vertex = null)
        {
            Player = player;
            Id = id;
            Vertex = vertex;
        }

        public IPlayer Player { get; }
        public int Id { get; }
        public Vertex Vertex { get; }
        public PieceType PieceType => CityType;
        public bool AffectsRoad => false;
        public IResourceList Cost => new CityCost();
        public int VictoryPoints => 2;

        public bool IsAt(Location location) => Vertex.Locations.Contains(location);

        public IResourceList Produce(IHex hex)
        {
            return new ResourceList(new[] { hex.Produce(), hex.Produce() });
        }

        public void AddToPlayer(IPlayer player)
        {
            player.Cities[Vertex] = this;
            player.Pieces.Add(this);
            player.Stock[CityType].Remove(this);
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
            player.Cities.Remove(Vertex);
            player.Pieces.Remove(this);
            player.Stock[CityType].Add(this);
            player.VictoryPoints.Remove(this);
            player.VertexPieces[Vertex].Remove(this);
            player.Producers.Remove(this);
        }

        public void AddToBoard(IBoard board)
        {
            board.CitiesByVertex[Vertex] = this;
            board.PiecesByVertex[Vertex] = this;
            board.Pieces.Add(this);
            board.Producers.Add(this);
        }

        public void RemoveFromBoard(IBoard board)
        {
            board.CitiesByVertex.Remove(Vertex);
            board.PiecesByVertex.Remove(Vertex);
            board.Pieces.Remove(this);
            board.Producers.Remove(this);
        }

    }
}