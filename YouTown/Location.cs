using System;
using System.Collections.Generic;

namespace YouTown
{
    /// <summary>
    /// Uses three-axis system 
    /// </summary>
    /// <seealso cref="http://www.redblobgames.com/grids/hexagons/"/>
    public class Location
    {
        private List<Location> _neighbors;
        private List<Edge> _edges;
        private List<Point> _points;
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

        public int X { get; }
        public int Y { get; }
        public int Z { get; }

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

        public IList<Point> Points
        {
            get
            {
                if (_points != null)
                {
                    return _points;
                }
                var neighbors = Neighbors;
                _points = new List<Point>
                {
                    new Point(this, neighbors[0], neighbors[1]),
                    new Point(this, neighbors[1], neighbors[2]),
                    new Point(this, neighbors[2], neighbors[3]),
                    new Point(this, neighbors[3], neighbors[4]),
                    new Point(this, neighbors[4], neighbors[5]),
                    new Point(this, neighbors[5], neighbors[0])
                };
                return _points;
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
