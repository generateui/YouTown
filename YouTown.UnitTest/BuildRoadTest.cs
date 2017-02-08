using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using YouTown.GameAction;

namespace YouTown.UnitTest
{
    [TestClass]
    public class BuildRoadTest
    {
        [TestMethod]
        public void SimpleSetup_CanBuild()
        {
            var playerMock = new Mock<IPlayer>();
            playerMock.Setup(p => p.IsOnTurn).Returns(true);
            var player = playerMock.Object;

            var roadToBuildMock = new Mock<Road>(player, 0, null);
            var roadToBuild = roadToBuildMock.Object;

            var existingRoadMock = new Mock<Road>(player, 0, null);
            var existingRoad = existingRoadMock.Object;

            var location1 = new Location(0, 0, 0);
            var hexMock1 = new Mock<IHex>();
            hexMock1.Setup(h => h.Location).Returns(location1);

            var location2 = new Location(1, -1, 0);
            var hexMock2 = new Mock<IHex>();
            hexMock2.Setup(h => h.Location).Returns(location2);

            var location3 = new Location(1, 0, -1);

            var edge = new Edge(location1, location2);
            var edgeExistingRoad = new Edge(location2, location3);

            var userMock = new Mock<IUser>();
            var user = userMock.Object;

            var stock = new Dictionary<PieceType, IList<IPiece>>
            {
                [Road.RoadType] = new List<IPiece> { roadToBuild }
            };
            var roads = new Dictionary<Edge, Road> {{edgeExistingRoad, existingRoad}};
            var playerPieces = new HashSet<IPiece>();
            playerMock.Setup(p => p.Stock).Returns(stock);
            playerMock.Setup(p => p.Roads).Returns(roads);
            playerMock.Setup(p => p.Towns).Returns(new Dictionary<Point, Town>());
            playerMock.Setup(p => p.Cities).Returns(new Dictionary<Point, City>());
            playerMock.Setup(p => p.User).Returns(user);
            playerMock.Setup(p => p.Pieces).Returns(playerPieces);
            playerMock.Setup(p => p.EdgePieces).Returns(new Dictionary<Edge, IList<IEdgePiece>>());

            var boardMock = new Mock<IBoard>();

            var hexesByLocation = new Dictionary<Location, IHex>
            {
                {location1, hexMock1.Object},
                {location2, hexMock2.Object},
            };
            boardMock.Setup(b => b.HexesByLocation).Returns(hexesByLocation);
            var piecesByEdge = new Dictionary<Edge, IEdgePiece>
            {
                {edgeExistingRoad, existingRoad }
            };
            boardMock.Setup(b => b.PiecesByEdge).Returns(piecesByEdge);
            boardMock.Setup(b => b.RoadsByEdge).Returns(new Dictionary<Edge, Road>());
            boardMock.Setup(b => b.Pieces).Returns(new HashSet<IPiece>());

            var board = boardMock.Object;

            var gameMock = new Mock<IGame>();
            gameMock.Setup(g => g.Board).Returns(board);
            var playTurnsMock = new Mock<PlayTurns>();
            var playTurns = playTurnsMock.Object;
            gameMock.Setup(g => g.GamePhase).Returns(playTurns);
            gameMock.Setup(g => g.PlayTurns).Returns(playTurns);
            var game = gameMock.Object;

            var buildRoad = new BuildRoad(0, player, edge);

            Assert.IsTrue(buildRoad.Validate(game).IsValid);

            buildRoad.Perform(game);
            Assert.AreEqual(2, roads.Count);
            Assert.IsTrue(roads.ContainsValue(roadToBuild));
            Assert.IsTrue(playerPieces.Contains(roadToBuild));
        }
    }
}
