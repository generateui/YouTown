using System.Collections.Generic;

namespace YouTown
{
    public interface IPlayer
    {
        IUser User { get; }
        string Color { get; } //TODO: introduce PlayerColor
        bool IsOnTurn { get; set; }
        int TotalVictoryPoints { get; }

        ISet<IPiece> Pieces { get; }
        IDictionary<Edge, IList<IEdgePiece>> EdgePieces { get; }
        IDictionary<Point, IList<IPointPiece>> PointPieces { get; }

        IResourceList Hand { get; }
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
}
