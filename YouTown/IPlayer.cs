﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace YouTown
{
    public sealed class PlayerColor
    {
        public static readonly PlayerColor White = new PlayerColor("White");
        public static readonly PlayerColor Green = new PlayerColor("Green");
        public static readonly PlayerColor Red = new PlayerColor("Red");
        public static readonly PlayerColor Blue = new PlayerColor("Blue");
        public static readonly PlayerColor Orange = new PlayerColor("Orange");
        public static readonly PlayerColor Brown = new PlayerColor("Brown");
        public static readonly IReadOnlyList<PlayerColor> AllColors = new List<PlayerColor>
        {
            White, Green, Red, Blue, Orange, Brown
        };
        private readonly string _color;

        private PlayerColor(string color)
        {
            _color = color;
        }

        public string Value => _color;

        public static PlayerColor Parse(string playerColorString)
        {
            var parsed = AllColors.FirstOrDefault(pc => pc.Value == playerColorString);
            if (parsed != null)
            {
                return parsed;
            }
            var colorStrings = AllColors.Select(pc => pc.Value);
            var commaSeparatedColorStrings = string.Join(",", colorStrings);
            throw new ArgumentException($"Got: [{playerColorString}. Expected one of: [{commaSeparatedColorStrings}]");
        }

        private bool Equals(PlayerColor other)
        {
            return string.Equals(_color, other._color);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is PlayerColor && Equals((PlayerColor) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return _color?.GetHashCode() ?? 0;
        }

        /// <inheritdoc />
        public override string ToString() => _color;
    }

    public interface IPlayer : IGameItem
    {
        IUser User { get; }
        PlayerColor Color { get; } //TODO: introduce PlayerColor
        bool IsOnTurn { get; set; }
        int TotalVictoryPoints { get; }

        ISet<IPiece> Pieces { get; }
        ISet<IProducer> Producers { get; }
        IDictionary<Edge, IList<IEdgePiece>> EdgePieces { get; }
        IDictionary<Vertex, IList<IVertexPiece>> VertexPieces { get; }

        MutableResourceList Hand { get; }
        IPortList Ports { get; }
        IList<IDevelopmentCard> DevelopmentCards { get; }
        IList<IDevelopmentCard> PlayedDevelopmentCards { get; }
        IList<IVictoryPoint> VictoryPoints { get; }
        IList<Soldier> Soldiers { get; }
        IDictionary<PieceType, IList<IPiece>> Stock { get; }

        IDictionary<Edge, Road> Roads { get; }
        IDictionary<Vertex, Town> Towns { get; }
        IDictionary<Vertex, City> Cities { get; }

        void GainResourcesFrom(IResourceList from, IResourceList toGain, IObscurable obscurable);
        void LooseResourcesTo(IResourceList to, IResourceList toLoose, IObscurable obscurable);
    }

    public class Player : IPlayer
    {
        public Player(int id, PlayerColor color, IUser user)
        {
            Id = id;
            User = user;
            Color = color;
        }

        public int Id { get; }
        public IUser User { get; }
        public PlayerColor Color { get; }
        public bool IsOnTurn { get; set; } = false;
        public int TotalVictoryPoints => VictoryPoints.Sum(vp => vp.VictoryPoints);
        public ISet<IPiece> Pieces { get; } = new HashSet<IPiece>();
        public ISet<IProducer> Producers { get; } = new HashSet<IProducer>();
        public IDictionary<Edge, IList<IEdgePiece>> EdgePieces { get; } = new Dictionary<Edge, IList<IEdgePiece>>();
        public IDictionary<Vertex, IList<IVertexPiece>> VertexPieces { get; } = new Dictionary<Vertex, IList<IVertexPiece>>();
        public MutableResourceList Hand { get; } = new MutableResourceList();
        public IPortList Ports { get; } = new PortList();
        public IList<IDevelopmentCard> DevelopmentCards { get; } = new List<IDevelopmentCard>();
        public IList<IDevelopmentCard> PlayedDevelopmentCards { get; } = new List<IDevelopmentCard>();
        public IList<IVictoryPoint> VictoryPoints { get; } = new List<IVictoryPoint>();
        public IList<Soldier> Soldiers { get; } = new List<Soldier>();
        public IDictionary<PieceType, IList<IPiece>> Stock { get; } = new Dictionary<PieceType, IList<IPiece>>();
        public IDictionary<Edge, Road> Roads { get; } = new Dictionary<Edge, Road>();
        public IDictionary<Vertex, Town> Towns { get; } = new Dictionary<Vertex, Town>();
        public IDictionary<Vertex, City> Cities { get; } = new Dictionary<Vertex, City>();

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
