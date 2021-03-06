﻿using System;
using System.Collections.Generic;

namespace YouTown
{
    /// <summary>
    /// Uses three-axis system 
    /// </summary>
    /// Another name for this may be a *tri-ordinate* as this type is 
    /// represented by three axis values x, y and z.
    /// <seealso cref="http://www.redblobgames.com/grids/hexagons/"/>
    public sealed class Location
    {
        private List<Location> _neighbors;
        private List<Edge> _edges;
        private List<Vertex> _vertices;
        private int _hashCode;
        private bool _hashCodeCalculated;

        public Location(int x, int y, int z)
        {
            if (x + y + z != 0)
            {
                throw new ArgumentException("Expect sum of ordinates to be zero");
            }
            X = x;
            Y = y;
            Z = z;
        }

        public Location(LocationData data)
        {
            X = data.X;
            Y = data.Y;
            Z = data.Z;
        }

        public int X { get; }
        public int Y { get; }
        public int Z { get; }

        public LocationData ToData() => new LocationData
        {
            X = X,
            Y = Y,
            Z = Z
        };

        public IList<Location> Neighbors
        {
            get
            {
                if (_neighbors != null)
                {
                    return _neighbors;
                }
                // see http://www.redblobgames.com/grids/hexagons/
                _neighbors = new List<Location>
                {
                    new Location(X + 1, Y - 1, Z + 0),
                    new Location(X + 1, Y - 0, Z - 1),
                    new Location(X + 0, Y + 1, Z - 1),
                    new Location(X - 1, Y + 1, Z + 0),
                    new Location(X - 1, Y + 0, Z + 1),
                    new Location(X + 0, Y - 1, Z + 1),
                };
                return _neighbors;
            }
        }

        public IList<Edge> Edges
        {
            get
            {
                if (_edges != null)
                {
                    return _edges;
                }
                var neighbors = Neighbors;
                _edges = new List<Edge>
                {
                    new Edge(this, neighbors[0]),
                    new Edge(this, neighbors[1]),
                    new Edge(this, neighbors[2]),
                    new Edge(this, neighbors[3]),
                    new Edge(this, neighbors[4]),
                    new Edge(this, neighbors[5]),
                };
                return _edges;
            }
        }

        public IList<Vertex> Vertices
        {
            get
            {
                if (_vertices != null)
                {
                    return _vertices;
                }
                var neighbors = Neighbors;
                _vertices = new List<Vertex>
                {
                    new Vertex(this, neighbors[0], neighbors[1]),
                    new Vertex(this, neighbors[1], neighbors[2]),
                    new Vertex(this, neighbors[2], neighbors[3]),
                    new Vertex(this, neighbors[3], neighbors[4]),
                    new Vertex(this, neighbors[4], neighbors[5]),
                    new Vertex(this, neighbors[5], neighbors[0])
                };
                return _vertices;
            }
        }

        protected bool Equals(Location other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Location) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
             // Pack 3 ordinates x, y and z into one 32-bit positive int
            if (!_hashCodeCalculated)
            {
                _hashCode = (X + 512) | ((Y + 512) << 10) | ((Z + 512) << 20);
                _hashCodeCalculated = true;
            }
            return _hashCode;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"X: {X}, Y: {Y}, Z: {Z}";
        }
    }
}
