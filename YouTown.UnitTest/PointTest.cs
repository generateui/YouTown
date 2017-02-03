using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YouTown.UnitTest
{
    [TestClass]
    public class PointTest
    {
        [TestMethod]
        public void Neighbors_AreCorrect3Neighbors()
        {
            var point = new Point(new Location(0,0,0), new Location(1, -1, 0), new Location(1,0,-1));
            var neighbors = point.Neighbors;

            Assert.AreEqual(3, neighbors.Count);
            Assert.IsTrue(neighbors.Contains(new Point(new Location(0,0,0), new Location(1, 0, -1), new Location(0, 1, -1))));
            Assert.IsTrue(neighbors.Contains(new Point(new Location(0,0,0), new Location(1, -1, -0), new Location(0, -1, 1))));
            Assert.IsTrue(neighbors.Contains(new Point(new Location(1,-1,0), new Location(2, -1, -1), new Location(1, 0, -1))));
        }

        [TestMethod]
        public void Edges_Are3CorrectEdges()
        {
            var point = new Point(new Location(0,0,0), new Location(1, -1, 0), new Location(1,0,-1));
            var edges = point.Edges;

            Assert.AreEqual(3, edges.Count);
            Assert.IsTrue(edges.Contains(new Edge(new Location(0,  0, 0), new Location(1, -1,  0))));
            Assert.IsTrue(edges.Contains(new Edge(new Location(0,  0, 0), new Location(1,  0, -1))));
            Assert.IsTrue(edges.Contains(new Edge(new Location(1, -1, 0), new Location(1,  0, -1))));
        }

        [TestMethod]
        public void Equals_IsCommutative()
        {
            var point1 = new Point(new Location(0, 0, 0), new Location(1, 0, -1), new Location(0, 1, -1)); // 123
            var point2 = new Point(new Location(0, 0, 0),new Location(0, 1, -1), new Location(1, 0, -1)); // 132
            var point3 = new Point(new Location(1, 0, -1), new Location(0, 0, 0), new Location(0, 1, -1)); // 213
            var point4 = new Point(new Location(1, 0, -1), new Location(0, 1, -1), new Location(0, 0, 0)); // 231
            var point5 = new Point(new Location(0, 1, -1), new Location(0, 0, 0), new Location(1, 0, -1)); // 312
            var point6 = new Point(new Location(0, 1, -1), new Location(1, 0, -1), new Location(0, 0, 0)); // 321
            var set = new HashSet<Point>
            {
                point1,
                point2,
                point3,
                point4,
                point5,
                point6,
            };

            Assert.AreEqual(1, set.Count);
            Assert.AreEqual(point1, point2);
            Assert.AreEqual(point1, point3);
            Assert.AreEqual(point1, point4);
            Assert.AreEqual(point1, point5);
            Assert.AreEqual(point1, point6);
            Assert.AreEqual(point2, point1);
            Assert.AreEqual(point2, point3);
            Assert.AreEqual(point2, point4);
            Assert.AreEqual(point2, point5);
            Assert.AreEqual(point2, point6);
            Assert.AreEqual(point3, point1);
            Assert.AreEqual(point3, point2);
            Assert.AreEqual(point3, point4);
            Assert.AreEqual(point3, point5);
            Assert.AreEqual(point3, point6);
            Assert.AreEqual(point4, point1);
            Assert.AreEqual(point4, point2);
            Assert.AreEqual(point4, point3);
            Assert.AreEqual(point4, point5);
            Assert.AreEqual(point4, point6);
            Assert.AreEqual(point5, point1);
            Assert.AreEqual(point5, point2);
            Assert.AreEqual(point5, point3);
            Assert.AreEqual(point5, point4);
            Assert.AreEqual(point5, point6);
            Assert.AreEqual(point6, point1);
            Assert.AreEqual(point6, point2);
            Assert.AreEqual(point6, point3);
            Assert.AreEqual(point6, point4);
            Assert.AreEqual(point6, point5);
        }
    }
}
