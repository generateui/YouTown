using System.Collections.Generic;
using System.Linq;

namespace YouTown
{
    public interface IBoardForPlay
    {
        IReadOnlyDictionary<Location, IHex> HexesByLocation { get; }
        IDictionary<Edge, Road> RoadsByEdge { get; }
        IDictionary<Vertex, Town> TownsByVertex { get; }
        IDictionary<Vertex, City> CitiesByVertex { get; }
        IDictionary<Vertex, IVertexPiece> PiecesByVertex { get; }
        IDictionary<Edge, IEdgePiece> PiecesByEdge { get; }
        ISet<IProducer> Producers { get; }
        ISet<IPiece> Pieces { get; }
        ISet<IPort> Ports { get; }
        Robber Robber { get; set; } // TODO: does this belong to IGame or IBoard?
    }

    public class BoardForPlay : IBoardForPlay
    {
        public BoardForPlay(IReadOnlyDictionary<Location, IHex> hexesByLocation)
        {
            HexesByLocation = hexesByLocation;
        }

        public BoardForPlay(BoardForPlayData data, IRepository repo)
        {
            HexesByLocation = data.Hexes.Select(h => h.FromData()).ToDictionary(h => h.Location, h => h);
            Robber = data.Robber != null ? new Robber(data.Robber) : null;
            Pieces = data.PieceIds.FromRepo<IPiece>(repo).ToSet();
            Producers = data.ProducerIds.FromRepo<IProducer>(repo).ToSet();
            RoadsByEdge = data.RoadIds.FromRepo<Road>(repo).ToDictionary(r => r.Edge, r => r);
            TownsByVertex = data.TownIds.FromRepo<Town>(repo).ToDictionary(t => t.Vertex, t => t);
            CitiesByVertex = data.CityIds.FromRepo<City>(repo).ToDictionary(c => c.Vertex, c => c);
            PiecesByEdge = data.EdgePieceIds.FromRepo<IEdgePiece>(repo).ToDictionary(ep => ep.Edge, ep => ep);
            PiecesByVertex = data.VertexPieceIds.FromRepo<IVertexPiece>(repo).ToDictionary(vp => vp.Vertex, vp => vp);
        }

        public IReadOnlyDictionary<Location, IHex> HexesByLocation { get; }
        public ISet<IPort> Ports => HexesByLocation.Values.Where(h => h.Port != null).Select(h => h.Port).ToSet();
        public Robber Robber { get; set; }
        public ISet<IPiece> Pieces { get; } = new HashSet<IPiece>();
        public ISet<IProducer> Producers { get; } = new HashSet<IProducer>();
        public IDictionary<Edge, Road> RoadsByEdge { get; } = new Dictionary<Edge, Road>();
        public IDictionary<Vertex, Town> TownsByVertex { get; } = new Dictionary<Vertex, Town>();
        public IDictionary<Vertex, City> CitiesByVertex { get; } = new Dictionary<Vertex, City>();
        public IDictionary<Vertex, IVertexPiece> PiecesByVertex { get; } = new Dictionary<Vertex, IVertexPiece>();
        public IDictionary<Edge, IEdgePiece> PiecesByEdge { get; } = new Dictionary<Edge, IEdgePiece>();
    }
}