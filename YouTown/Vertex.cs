using System.Collections.Generic;
using System.Linq;

namespace YouTown
{
    public class Vertex
    {
        private readonly Location _location1;
        private readonly Location _location2;
        private readonly Location _location3;
        private List<Edge> _edges;
        private List<Vertex> _neighbors;
        private List<Location> _locations;

        public Vertex(Location location1, Location location2, Location location3)
        {
            _location1 = location1;
            _location2 = location2;
            _location3 = location3;
        }

        public IList<Edge> Edges
        {
            get
            {
                if (_edges == null)
                {
                    _edges = new List<Edge>
                    {
                        new Edge(_location1, _location2),
                        new Edge(_location1, _location3),
                        new Edge(_location2, _location3),
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
                        _location1,
                        _location2,
                        _location3
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
                    .Concat(_location1.Neighbors)
                    .Concat(_location2.Neighbors)
                    .GroupBy(hl => hl)
                    .Where(g => g.Count() == 2)
                    .Select(g => g.Key)
                    .FirstOrDefault(hl => !hl.Equals(_location3));

                var firstThirdNeighbor = Enumerable
                    .Empty<Location>()
                    .Concat(_location1.Neighbors)
                    .Concat(_location3.Neighbors)
                    .GroupBy(hl => hl)
                    .Where(g => g.Count() == 2)
                    .Select(g => g.Key)
                    .FirstOrDefault(hl => !hl.Equals(_location2));

                var secondThirdNeighbor = Enumerable
                    .Empty<Location>()
                    .Concat(_location2.Neighbors)
                    .Concat(_location3.Neighbors)
                    .GroupBy(hl => hl)
                    .Where(g => g.Count() == 2)
                    .Select(g => g.Key)
                    .FirstOrDefault(hl => !hl.Equals(_location1));

                _neighbors = new List<Vertex>
                {
                    new Vertex(_location1, _location2, firstSecondNeighbor),
                    new Vertex(_location1, _location3, firstThirdNeighbor),
                    new Vertex(_location2, _location3, secondThirdNeighbor)
                };
                return _neighbors;
            }
        }

        protected bool Equals(Vertex other)
        {
            return (Equals(_location1, other._location1) &&
                    Equals(_location2, other._location2) &&
                    Equals(_location3, other._location3)) ||

                   (Equals(_location1, other._location1) &&
                    Equals(_location2, other._location3) &&
                    Equals(_location3, other._location2)) ||

                   (Equals(_location1, other._location2) &&
                    Equals(_location2, other._location1) &&
                    Equals(_location3, other._location3)) ||

                   (Equals(_location1, other._location2) &&
                    Equals(_location2, other._location3) &&
                    Equals(_location3, other._location1)) ||

                   (Equals(_location1, other._location3) &&
                    Equals(_location2, other._location2) &&
                    Equals(_location3, other._location1)) ||

                   (Equals(_location1, other._location3) &&
                    Equals(_location2, other._location1) &&
                    Equals(_location3, other._location2));
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
                var hashCode = _location1?.GetHashCode() ?? 0;
                hashCode = (hashCode*397) * (_location2?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) * (_location3?.GetHashCode() ?? 0);
                return hashCode;
            }
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"Location1: {_location1}, Location2: {_location2}, Location3: {_location3}";
        }
    }
}
