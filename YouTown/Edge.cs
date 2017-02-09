using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace YouTown
{
    public class Edge
    {
        private List<Location> _locations;
        private List<Vertex> _vertices;
        private List<Edge> _neighbors;

        public Edge(Location first, Location second)
        {
            Location1 = first;
            Location2 = second;
            Debug.Assert(Location1.X == Location2.X || Location1.Y == Location2.Y || Location1.Z == Location2.Z);
        }

        public Location Location1 { get; }
        public Location Location2 { get; }

        public Vertex Vertex1 => Vertices[0];
        public Vertex Vertex2 => Vertices[1];

        public IList<Location> Locations
        {
            get
            {
                if (_locations == null)
                {
                    _locations = new List<Location>
                    {
                        Location1,
                        Location2
                    };
                }
                return _locations;
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
                var neighbors = Location1.Neighbors
                    .Concat(Location2.Neighbors)
                    .GroupBy(hl => hl)
                    .Where(g => g.Count() == 2)
                    .Select(g => g.Key)
                    .ToList();
                _vertices = new List<Vertex>
                {
                    new Vertex(Location1, Location2, neighbors[0]),
                    new Vertex(Location1, Location2, neighbors[1]),
                };
                return _vertices;
            }
        }

        public IList<Edge> Neighbors
        {
            get
            {
                if (_neighbors != null)
                {
                    return _neighbors;
                }
                var neighbors = Location1.Neighbors
                    .Concat(Location2.Neighbors)
                    .GroupBy(hl => hl)
                    .Where(g => g.Count() == 2)
                    .Select(g => g.Key)
                    .ToList();
                _neighbors = new List<Edge>
                {
                    new Edge(Location1, neighbors[0]),
                    new Edge(Location1, neighbors[1]),
                    new Edge(Location2, neighbors[0]),
                    new Edge(Location2, neighbors[1]),
                };
                return _neighbors;
            }
        }

        public bool Connects(Edge other)
        {
            var neighbors = Neighbors;
            return neighbors.Contains(other);
        }

        protected bool Equals(Edge other)
        {
            return (Equals(Location1, other.Location1) && Equals(Location2, other.Location2)) ||
                   (Equals(Location1, other.Location2) && Equals(Location2, other.Location1));
        }

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Edge) obj);
        }

        /// <summary>Serves as the default hash function. </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((Location1?.GetHashCode() ?? 0)*397) + ((Location2?.GetHashCode() ?? 0)*397);
            }
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"Location1: {Location1}, Location2: {Location2}";
        }
    }
}
