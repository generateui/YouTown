﻿namespace YouTown
{
    /// <summary>
    /// Typed name for a piece type
    /// </summary>
    /// Used as type differentiator to store separated lists by type,
    /// e.g. dictionaries
    public class PieceType
    {
        private readonly string _pieceType;

        public PieceType(string pieceType)
        {
            _pieceType = pieceType;
        }

        private bool Equals(PieceType other)
        {
            return string.Equals(_pieceType, other._pieceType);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PieceType) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return _pieceType?.GetHashCode() ?? 0;
        }
    }

    /// <summary>
    /// In-game piece that belongs to a player
    /// </summary>
    public interface IPiece : IGameItem
    {
        PieceType PieceType { get; }
        IPlayer Player { get; }
        bool AffectsRoad { get; }

        void AddToPlayer(IPlayer player);
        void RemoveFromPlayer(IPlayer player);
        void AddToBoard(IBoardForPlay board);
        void RemoveFromBoard(IBoardForPlay board);

        IResourceList Cost { get; }
    }

    /// <summary>
    /// Piece producing resource(s) after a dice roll has been cast
    /// </summary>
    public interface IProducer : IGameItem
    {
        IPlayer Player { get; }
        IResourceList Produce(IHex hex);
        bool IsAt(Location location);
    }

    /// <summary>
    /// Piece associated with a Vertex on the board
    /// </summary>
    public interface IVertexPiece : IGameItem
    {
        Vertex Vertex { get; }
    }

    /// <summary>
    /// Piece residing on the edge
    /// </summary>
    public interface IEdgePiece : IGameItem
    {
        Edge Edge { get; }
        
        // TODO: when implementing longestroad
//        bool ConnectsWithRoad { get; }
//        bool ConnectsWithShip { get; }
//        bool ConnectsWithBridge { get; }
    }

    /// <summary>
    /// Any piece casting victorypoints for the player
    /// </summary>
    public interface IVictoryPoint : IGameItem
    {
        int VictoryPoints { get; }
    }
}
