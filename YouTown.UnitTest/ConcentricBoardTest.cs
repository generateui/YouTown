using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YouTown.UnitTest
{
    [TestClass]
    public class ConcentricBoardTest
    {
        [TestMethod]
        public void AfterNew_PortsArePlaced()
        {
            var cb = new ConcentricBoard(4, string.Empty);

            var waterHexes = cb.HexesByLocation.Values.Where(h => h is Water);
            var randomHexes = cb.HexesByLocation.Values.Where(h => h is RandomHex);
            Assert.AreEqual(18, waterHexes.Count());
            Assert.AreEqual(19, randomHexes.Count());

            var waterHexesWithPort = cb.HexesByLocation.Values.Count(h => h.Port != null);
            Assert.AreEqual(10, waterHexesWithPort);
        }

        [TestMethod]
        public void Setup_UsesAllProvidedItems()
        {
            var cb = new ConcentricBoard(4, string.Empty);
            
            var chitList = new List<IChit>
            {
                new Chit2(),
                new Chit3(), new Chit3(),
                new Chit4(), new Chit4(),
                new Chit5(), new Chit5(),
                new Chit6(), new Chit6(),
                new Chit8(), new Chit8(),
                new Chit9(), new Chit9(),
                new Chit10(), new Chit10(),
                new Chit11(), new Chit11(),
                new Chit12()
            };
            var hexList = new List<IHex>
            {
                new Forest(),new Forest(),new Forest(),new Forest(),
                new Field(),new Field(),new Field(),new Field(),
                new Pasture(),new Pasture(),new Pasture(),new Pasture(),
                new Hill(),new Hill(),new Hill(),
                new Mountain(),new Mountain(),new Mountain(),
            };
            var portList = new List<IPort>
            {
                new ThreeToOnePort(),new ThreeToOnePort(),new ThreeToOnePort(),new ThreeToOnePort(),
                new WheatPort(),
                new TimberPort(),
                new ClayPort(),
                new SheepPort(),
                new OrePort()
            };

            var setupBoard = cb.Setup(portList, chitList, hexList, new DotNetRandom());

            var waterWithPort = setupBoard.HexesByLocation.Values.Where(h => h.Port != null);
            Assert.AreEqual(9, waterWithPort.Count());

            var hexesWithChit = setupBoard.HexesByLocation.Values.Where(h => h.Chit != null);
            Assert.AreEqual(18, hexesWithChit.Count());

            var waterHexes = setupBoard.HexesByLocation.Values.Where(h => h is Water);
            var randomHexes = setupBoard.HexesByLocation.Values.Where(h => h is RandomHex);
            Assert.AreEqual(18, waterHexes.Count());
            Assert.AreEqual(0, randomHexes.Count());
        }
    }
}
