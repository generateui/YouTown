using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YouTown.UnitTest
{
    [TestClass]
    public class EdgeTest
    {
        [TestMethod]
        public void Neighbors_AreCorrect4Neighbors()
        {
            var edge = new Edge(new Location(0,0,0), new Location(1, -1, 0));
            var neighbors = edge.Neighbors;

            Assert.AreEqual(4, neighbors.Count);
            Assert.IsTrue(neighbors.Contains(new Edge(new Location(1,  0, -1), new Location(1, -1, 0))));
            Assert.IsTrue(neighbors.Contains(new Edge(new Location(1,  0, -1), new Location(0,  0, 0))));
            Assert.IsTrue(neighbors.Contains(new Edge(new Location(0, -1,  1), new Location(1, -1, 0))));
            Assert.IsTrue(neighbors.Contains(new Edge(new Location(0, -1,  1), new Location(0,  0, 0))));
        }

        [TestMethod]
        public void Points_AreCorrect2Points()
        {
            var edge = new Edge(new Location(0,0,0), new Location(1, -1, 0));
            var points = edge.Points;

            Assert.IsTrue(points.Contains(new Point(new Location(0,0,0), new Location(1, -1, 0), new Location(1,  0, -1))));
            Assert.IsTrue(points.Contains(new Point(new Location(0,0,0), new Location(1, -1, 0), new Location(0, -1,  1))));
        }

        [TestMethod]
        public void Equals_IsCommutative()
        {
            var edge1 = new Edge(new Location(0,0,0),new Location(1, -1, 0));
            var edge2 = new Edge(new Location(1,-1, 0),new Location(0, 0, 0));

            Assert.AreEqual(edge1, edge2);
            Assert.AreEqual(edge2, edge1);
            Assert.AreEqual(edge1.GetHashCode(), edge2.GetHashCode());
        }

    }
}
