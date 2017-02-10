using System.Collections.Generic;
using System.Linq;

namespace YouTown
{
    public class Vertex
    {
        private List<Edge> _edges;
        private List<Vertex> _neighbors;
        private List<Location> _locations;

        public Vertex(Location location1, Location location2, Location location3)
        {
            Location1 = location1;
            Location2 = location2;
            Location3 = location3;
        }

        public Location Location1 { get; }
        public Location Location2 { get; }
        public Location Location3 { get; }

        public IList<Edge> Edges
        {
            get
            {
                if (_edges == null)
                {
                    _edges = new List<Edge>
                    {
                        new Edge(Location1, Location2),
                        new Edge(Location1, Location3),
                        new Edge(Location2, Location3),
                    };
                }
                return _edges;
            }
        }

        public IList<Location> Locations
        {
            get
            {
                if (_locations == null)
                {
                    _locations = new List<Location>
                    {
                        Location1,
                        Location2,
                        Location3
                    };
                }
                return _locations;
            }
        }

        public IList<Vertex> Neighbors
        {
            get
            {
                if (_neighbors != null)
                {
                    return _neighbors;
                }

                // This can probably be simplified
                var firstSecondNeighbor = Enumerable
                    .Empty<Location>()
                    .Concat(Location1.Neighbors)
                    .Concat(Location2.Neighbors)
                    .GroupBy(hl => hl)
                    .Where(g => g.Count() == 2)
                    .Select(g => g.Key)
                    .FirstOrDefault(hl => !hl.Equals(Location3));

                var firstThirdNeighbor = Enumerable
                    .Empty<Location>()
                    .Concat(Location1.Neighbors)
                    .Concat(Location3.Neighbors)
                    .GroupBy(hl => hl)
                    .Where(g => g.Count() == 2)
                    .Select(g => g.Key)
                    .FirstOrDefault(hl => !hl.Equals(Location2));

                var secondThirdNeighbor = Enumerable
                    .Empty<Location>()
                    .Concat(Location2.Neighbors)
                    .Concat(Location3.Neighbors)
                    .GroupBy(hl => hl)
                    .Where(g => g.Count() == 2)
                    .Select(g => g.Key)
                    .FirstOrDefault(hl => !hl.Equals(Location1));

                _neighbors = new List<Vertex>
                {
                    new Vertex(Location1, Location2, firstSecondNeighbor),
                    new Vertex(Location1, Location3, firstThirdNeighbor),
                    new Vertex(Location2, Location3, secondThirdNeighbor)
                };
                return _neighbors;
            }
        }

        protected bool Equals(Vertex other)
        {
            return (Equals(Location1, other.Location1) &&
                    Equals(Location2, other.Location2) &&
                    Equals(Location3, other.Location3)) ||

                   (Equals(Location1, other.Location1) &&
                    Equals(Location2, other.Location3) &&
                    Equals(Location3, other.Location2)) ||

                   (Equals(Location1, other.Location2) &&
                    Equals(Location2, other.Location1) &&
                    Equals(Location3, other.Location3)) ||

                   (Equals(Location1, other.Location2) &&
                    Equals(Location2, other.Location3) &&
                    Equals(Location3, other.Location1)) ||

                   (Equals(Location1, other.Location3) &&
                    Equals(Location2, other.Location2) &&
                    Equals(Location3, other.Location1)) ||

                   (Equals(Location1, other.Location3) &&
                    Equals(Location2, other.Location1) &&
                    Equals(Location3, other.Location2));
        }

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Vertex) obj);
        }

        /// <summary>Serves as the default hash function. </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Location1?.GetHashCode() ?? 0;
                hashCode = (hashCode*397) * (Location2?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) * (Location3?.GetHashCode() ?? 0);
                return hashCode;
            }
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"Location1: {Location1}, Location2: {Location2}, Location3: {Location3}";
        }
    }
}
