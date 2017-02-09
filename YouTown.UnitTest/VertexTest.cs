using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YouTown.UnitTest
{
    [TestClass]
    public class VertexTest
    {
        [TestMethod]
        public void Neighbors_AreCorrect3Neighbors()
        {
            var vertex = new Vertex(new Location(0,0,0), new Location(1, -1, 0), new Location(1,0,-1));
            var neighbors = vertex.Neighbors;

            Assert.AreEqual(3, neighbors.Count);
            Assert.IsTrue(neighbors.Contains(new Vertex(new Location(0,0,0), new Location(1, 0, -1), new Location(0, 1, -1))));
            Assert.IsTrue(neighbors.Contains(new Vertex(new Location(0,0,0), new Location(1, -1, -0), new Location(0, -1, 1))));
            Assert.IsTrue(neighbors.Contains(new Vertex(new Location(1,-1,0), new Location(2, -1, -1), new Location(1, 0, -1))));
        }

        [TestMethod]
        public void Edges_Are3CorrectEdges()
        {
            var vertex = new Vertex(new Location(0,0,0), new Location(1, -1, 0), new Location(1,0,-1));
            var edges = vertex.Edges;

            Assert.AreEqual(3, edges.Count);
            Assert.IsTrue(edges.Contains(new Edge(new Location(0,  0, 0), new Location(1, -1,  0))));
            Assert.IsTrue(edges.Contains(new Edge(new Location(0,  0, 0), new Location(1,  0, -1))));
            Assert.IsTrue(edges.Contains(new Edge(new Location(1, -1, 0), new Location(1,  0, -1))));
        }

        [TestMethod]
        public void Equals_IsCommutative()
        {
            var vertex1 = new Vertex(new Location(0, 0, 0), new Location(1, 0, -1), new Location(0, 1, -1)); // 123
            var vertex2 = new Vertex(new Location(0, 0, 0),new Location(0, 1, -1), new Location(1, 0, -1)); // 132
            var vertex3 = new Vertex(new Location(1, 0, -1), new Location(0, 0, 0), new Location(0, 1, -1)); // 213
            var vertex4 = new Vertex(new Location(1, 0, -1), new Location(0, 1, -1), new Location(0, 0, 0)); // 231
            var vertex5 = new Vertex(new Location(0, 1, -1), new Location(0, 0, 0), new Location(1, 0, -1)); // 312
            var vertex6 = new Vertex(new Location(0, 1, -1), new Location(1, 0, -1), new Location(0, 0, 0)); // 321
            var set = new HashSet<Vertex>
            {
                vertex1,
                vertex2,
                vertex3,
                vertex4,
                vertex5,
                vertex6,
            };

            Assert.AreEqual(1, set.Count);
            Assert.AreEqual(vertex1, vertex2);
            Assert.AreEqual(vertex1, vertex3);
            Assert.AreEqual(vertex1, vertex4);
            Assert.AreEqual(vertex1, vertex5);
            Assert.AreEqual(vertex1, vertex6);
            Assert.AreEqual(vertex2, vertex1);
            Assert.AreEqual(vertex2, vertex3);
            Assert.AreEqual(vertex2, vertex4);
            Assert.AreEqual(vertex2, vertex5);
            Assert.AreEqual(vertex2, vertex6);
            Assert.AreEqual(vertex3, vertex1);
            Assert.AreEqual(vertex3, vertex2);
            Assert.AreEqual(vertex3, vertex4);
            Assert.AreEqual(vertex3, vertex5);
            Assert.AreEqual(vertex3, vertex6);
            Assert.AreEqual(vertex4, vertex1);
            Assert.AreEqual(vertex4, vertex2);
            Assert.AreEqual(vertex4, vertex3);
            Assert.AreEqual(vertex4, vertex5);
            Assert.AreEqual(vertex4, vertex6);
            Assert.AreEqual(vertex5, vertex1);
            Assert.AreEqual(vertex5, vertex2);
            Assert.AreEqual(vertex5, vertex3);
            Assert.AreEqual(vertex5, vertex4);
            Assert.AreEqual(vertex5, vertex6);
            Assert.AreEqual(vertex6, vertex1);
            Assert.AreEqual(vertex6, vertex2);
            Assert.AreEqual(vertex6, vertex3);
            Assert.AreEqual(vertex6, vertex4);
            Assert.AreEqual(vertex6, vertex5);
        }
    }
}
