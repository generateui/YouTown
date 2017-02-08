using System.Collections.Generic;
using System.Linq;

namespace YouTown
{
    public interface IPlayer
    {
        IUser User { get; }
        string Color { get; } //TODO: introduce PlayerColor
        bool IsOnTurn { get; set; }
        int TotalVictoryPoints { get; }

        ISet<IPiece> Pieces { get; }
        ISet<IProducer> Producers { get; }
        IDictionary<Edge, IList<IEdgePiece>> EdgePieces { get; }
        IDictionary<Point, IList<IPointPiece>> PointPieces { get; }

        MutableResourceList Hand { get; }
        IPortList Ports { get; }
        IList<IDevelopmentCard> DevelopmentCards { get; }
        IList<IDevelopmentCard> PlayedDevelopmentCards { get; }
        IList<IVictoryPoint> VictoryPoints { get; }
        IList<Soldier> Soldier { get; }
        IDictionary<PieceType, IList<IPiece>> Stock { get; }

        IDictionary<Edge, Road> Roads { get; }
        IDictionary<Point, Town> Towns { get; }
        IDictionary<Point, City> Cities { get; }

        void GainResourcesFrom(IResourceList from, IResourceList toGain, IObscurable obscurable);
        void LooseResourcesTo(IResourceList to, IResourceList toLoose, IObscurable obscurable);
    }

    public class Player : IPlayer
    {
        public Player(IUser user, string color)
        {
            User = user;
            Color = color;
        }

        public IUser User { get; }
        public string Color { get; }
        public bool IsOnTurn { get; set; } = false;
        public int TotalVictoryPoints => VictoryPoints.Sum(vp => vp.VictoryPoints);
        public ISet<IPiece> Pieces { get; } = new HashSet<IPiece>();
        public ISet<IProducer> Producers { get; } = new HashSet<IProducer>();
        public IDictionary<Edge, IList<IEdgePiece>> EdgePieces { get; } = new Dictionary<Edge, IList<IEdgePiece>>();
        public IDictionary<Point, IList<IPointPiece>> PointPieces { get; } = new Dictionary<Point, IList<IPointPiece>>();
        public MutableResourceList Hand { get; } = new MutableResourceList();
        public IPortList Ports { get; } = new PortList();
        public IList<IDevelopmentCard> DevelopmentCards { get; } = new List<IDevelopmentCard>();
        public IList<IDevelopmentCard> PlayedDevelopmentCards { get; } = new List<IDevelopmentCard>();
        public IList<IVictoryPoint> VictoryPoints { get; } = new List<IVictoryPoint>();
        public IList<Soldier> Soldier { get; } = new List<Soldier>();
        public IDictionary<PieceType, IList<IPiece>> Stock { get; } = new Dictionary<PieceType, IList<IPiece>>();
        public IDictionary<Edge, Road> Roads { get; } = new Dictionary<Edge, Road>();
        public IDictionary<Point, Town> Towns { get; } = new Dictionary<Point, Town>();
        public IDictionary<Point, City> Cities { get; } = new Dictionary<Point, City>();

        public void GainResourcesFrom(IResourceList @from, IResourceList toGain, IObscurable obscurable)
        {
            throw new System.NotImplementedException();
        }

        public void LooseResourcesTo(IResourceList to, IResourceList toLoose, IObscurable obscurable)
        {
            throw new System.NotImplementedException();
        }
    }
}
