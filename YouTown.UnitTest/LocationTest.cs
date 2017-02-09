using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YouTown.UnitTest
{
    [TestClass]
    public class LocationTest
    {
        private static class Center
        {
            public static readonly Location Location = new Location(0, 0, 0);
            public static readonly Location Neighbor0 = new Location( 1,  0, -1);
            public static readonly Location Neighbor1 = new Location( 0,  1, -1);
            public static readonly Location Neighbor2 = new Location(-1,  1,  0);
            public static readonly Location Neighbor3 = new Location(-1,  0,  1);
            public static readonly Location Neighbor4 = new Location( 0, -1,  1);
            public static readonly Location Neighbor5 = new Location(1,  -1,  0);
        }

        [TestMethod]
        public void Neighbors_AreCorrect6Neighbors()
        {
            var location = Center.Location;
            var neighbors = location.Neighbors;

            Assert.AreEqual(6, neighbors.Count);
            Assert.IsTrue(neighbors.Contains(Center.Neighbor0));
            Assert.IsTrue(neighbors.Contains(Center.Neighbor1));
            Assert.IsTrue(neighbors.Contains(Center.Neighbor2));
            Assert.IsTrue(neighbors.Contains(Center.Neighbor3));
            Assert.IsTrue(neighbors.Contains(Center.Neighbor4));
            Assert.IsTrue(neighbors.Contains(Center.Neighbor5));
        }

        [TestMethod]
        public void Vertices_AreCorrect6Vertices()
        {
            var location = Center.Location;
            var vertices = location.Vertices;

            Assert.AreEqual(6, vertices.Count);
            Assert.IsTrue(vertices.Contains(new Vertex(location, Center.Neighbor0, Center.Neighbor1)));
            Assert.IsTrue(vertices.Contains(new Vertex(location, Center.Neighbor1, Center.Neighbor2)));
            Assert.IsTrue(vertices.Contains(new Vertex(location, Center.Neighbor2, Center.Neighbor3)));
            Assert.IsTrue(vertices.Contains(new Vertex(location, Center.Neighbor3, Center.Neighbor4)));
            Assert.IsTrue(vertices.Contains(new Vertex(location, Center.Neighbor4, Center.Neighbor5)));
            Assert.IsTrue(vertices.Contains(new Vertex(location, Center.Neighbor5, Center.Neighbor0)));
        }

        [TestMethod]
        public void Edges_AreCorrect6Edges()
        {
            var location = Center.Location;
            var edges = location.Edges;

            Assert.AreEqual(6, edges.Count);
            Assert.IsTrue(edges.Contains(new Edge(location, Center.Neighbor0)));
            Assert.IsTrue(edges.Contains(new Edge(location, Center.Neighbor1)));
            Assert.IsTrue(edges.Contains(new Edge(location, Center.Neighbor2)));
            Assert.IsTrue(edges.Contains(new Edge(location, Center.Neighbor3)));
            Assert.IsTrue(edges.Contains(new Edge(location, Center.Neighbor4)));
            Assert.IsTrue(edges.Contains(new Edge(location, Center.Neighbor5)));
        }

        [TestMethod]
        public void CreateBoardFromNeighbors_EdgesVerticesKLocationsCountAreCorrect()
        {
            var center = Center.Location;
            var neighbors = center.Neighbors;
            var locations = neighbors
                .SelectMany(n => n.Neighbors)
                .Distinct()
                .ToList();
            var locationHashCodes = neighbors
                .SelectMany(l => l.Neighbors)
                .Select(l => l.GetHashCode())
                .Distinct()
                .ToList();

            var edges = locations.SelectMany(l => l.Edges).Distinct();
            var edgeHashCodes = locations
                .SelectMany(l => l.Edges)
                .Select(l => l.GetHashCode())
                .Distinct();

            var vertices = locations.SelectMany(l => l.Vertices).Distinct();
            var vertexHashCodes = locations
                .SelectMany(l => l.Vertices)
                .Select(p => p.GetHashCode())
                .Distinct();

            Assert.AreEqual(19, locations.Count);
            Assert.AreEqual(19, locationHashCodes.Count);

            Assert.AreEqual(72, edges.Count());
            Assert.AreEqual(72, edgeHashCodes.Count());

            Assert.AreEqual(54, vertices.Count());
            Assert.AreEqual(54, vertexHashCodes.Count());
        }

        [TestMethod]
        public void CreateBoardFromNeighborsLevel10_EdgesVerticesKLocationsCountAreCorrect()
        {
            Func<Location, int, IEnumerable<Location>> getNeighborsRecursive = (location, i) =>
            {
                var all = new List<Location> {location};
                i -= 1;
                while (i > 0)
                {
                    i--;
                    var neighborz = all.SelectMany(n => n.Neighbors).ToList();
                    all.AddRange(neighborz);
                }
                return all.Distinct();
            };

            Func<int, int> getLocationCountOfLevel = i =>
            {
                // 1 2  3  4  5  6 7
                // 1 7 19 37 61 91 127
                // 0 6 18 36 60 90 126
                //  6 12 18 24 30 36
                if (i < 1)
                {
                    return 0;
                }
                int x = i;
                int count = 1;
                while (x > 0)
                {
                    x--;
                    count += x*6;
                }
                return count;
            };
            Assert.AreEqual(1, getLocationCountOfLevel(1));
            Assert.AreEqual(7, getLocationCountOfLevel(2));
            Assert.AreEqual(19, getLocationCountOfLevel(3));
            Assert.AreEqual(37, getLocationCountOfLevel(4));
            Assert.AreEqual(61, getLocationCountOfLevel(5));
            Assert.AreEqual(91, getLocationCountOfLevel(6));
            Assert.AreEqual(127, getLocationCountOfLevel(7));

            Func<int, int> getEdgeCountOfLevel = i =>
            {
                // 1 2  3  4  5  6 7
                // 6 30 72 132
                //  24 42 60
                // 1 4  7  10
                if (i < 1)
                {
                    return 0;
                }
                int x = i;
                int count = 0;
                while (x > 0)
                {
                    count += (1 + ((x - 1)*3))*6;
                    x--;
                }
                return count;
            };
            Assert.AreEqual(6, getEdgeCountOfLevel(1));
            Assert.AreEqual(30, getEdgeCountOfLevel(2));
            Assert.AreEqual(72, getEdgeCountOfLevel(3));
            Assert.AreEqual(132, getEdgeCountOfLevel(4));

            Func<int, int> getVertexCountOfLevel = i =>
            {
                // 1 2  3  4  5  6 7
                // 6 24 54 96
                //  18 30 42
                // 1 3 5 7
                if (i < 1)
                {
                    return 0;
                }
                int x = i;
                int count = 0;
                while (x > 0)
                {
                    count += ((x*2) - 1)*6;
                    x--;
                }
                return count;
            };
            Assert.AreEqual(6, getVertexCountOfLevel(1));
            Assert.AreEqual(24, getVertexCountOfLevel(2));
            Assert.AreEqual(54, getVertexCountOfLevel(3));
            Assert.AreEqual(96, getVertexCountOfLevel(4));

            for (int level = 1; level < 10; level++)
            {
                var locations = getNeighborsRecursive(Center.Location, level);
                var locationHashCodes = locations.Select(l => l.GetHashCode());
                var locationCount = getLocationCountOfLevel(level);
                Assert.AreEqual(locationCount, locations.Count());
                Assert.AreEqual(locationCount, locationHashCodes.Count());

                var edges = locations.SelectMany(l => l.Edges).Distinct();
                var edgeHashCodes = edges.Select(e => e.GetHashCode());
                var edgeCount = getEdgeCountOfLevel(level);
                Assert.AreEqual(edgeCount, edges.Count());
                Assert.AreEqual(edgeCount, edgeHashCodes.Count());

                var vertices = locations.SelectMany(l => l.Vertices).Distinct();
                var vertexHashCodes = vertices.Select(p => p.GetHashCode());
                var vertexCount = getVertexCountOfLevel(level);
                Assert.AreEqual(vertexCount, vertices.Count());
                Assert.AreEqual(vertexCount, vertexHashCodes.Count());
            }
        }
    }
}
