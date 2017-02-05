using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YouTown.UnitTest
{
    [TestClass]
    public class PortListTest
    {
        [TestMethod]
        public void AllPortTypes_YieldCorectGold()
        {
            var resourceList = new ResourceList(new List<IResource>
            {
                new Timber(),new Timber(),new Timber(), // 1
                new Wheat(),new Wheat(),new Wheat(),new Wheat(),new Wheat(),new Wheat(), // 2
                new Sheep(), // 0
                new Clay(),new Clay(),new Clay(),new Clay(),new Clay(), // 1
                new Ore(),new Ore(), // 1
            });

            var portList = new PortList(new List<IPort>
            {
                new FourToOnePort(), new ThreeToOnePort(), new OrePort(), new TimberPort()
            });

            int goldAmount = portList.AmountGold(resourceList);
            Assert.AreEqual(5, goldAmount);
        }

        [TestMethod]
        public void FourToOnePort_YieldCorrectGold()
        {
            var resourceList = new ResourceList(new List<IResource>
            {
                new Timber(),new Timber(),new Timber(),new Timber(), // 1
                new Wheat(),new Wheat(),new Wheat(),new Wheat(),
                new Wheat(),new Wheat(),new Wheat(),new Wheat(), // 2
                new Sheep(), // 0
                new Clay(),new Clay(),new Clay(), // 0
                new Ore(),new Ore(), // 0
            });

            var portList = new PortList(new List<IPort>
            {
                new FourToOnePort(), 
            });

            int goldAmount = portList.AmountGold(resourceList);
            Assert.AreEqual(3, goldAmount);
        }

        [TestMethod]
        public void FourToOneAndThreeToOnePort_YieldCorrectGold()
        {
            var resourceList = new ResourceList(new List<IResource>
            {
                new Timber(),new Timber(),new Timber(),new Timber(), // 1
                new Wheat(),new Wheat(),new Wheat(),
                new Wheat(), new Wheat(),new Wheat(),
                new Wheat(),new Wheat(),new Wheat(), // 3
                new Sheep(), // 0
                new Clay(),new Clay(),new Clay(), // 1
                new Ore(),new Ore(), // 0
            });

            var portList = new PortList(new List<IPort>
            {
                new FourToOnePort(), new ThreeToOnePort()
            });

            int goldAmount = portList.AmountGold(resourceList);
            Assert.AreEqual(5, goldAmount);
        }

        [TestMethod]
        public void NotApplicableTwoToOnePort_YieldCorectGold()
        {
            var resourceList = new ResourceList(new List<IResource>
            {
                new Timber(),new Timber(),new Timber(),new Timber(), // 1
                new Wheat(),new Wheat(),new Wheat(), new Wheat(),
                new Wheat(),new Wheat(), new Wheat(),new Wheat(),
                new Wheat(), // 2
                // no sheep
                new Clay(),new Clay(),new Clay(), // 0
                new Ore(),new Ore(), // 0
            });

            var portList = new PortList(new List<IPort>
            {
                new FourToOnePort(), new SheepPort()
            });

            int goldAmount = portList.AmountGold(resourceList);
            Assert.AreEqual(3, goldAmount);
        }

        [TestMethod]
        public void WithFourToOnePort_HasFourToOnePort()
        {
            var portList = new PortList(new List<IPort>
            {
                new FourToOnePort(), new SheepPort()
            });

            Assert.IsTrue(portList.HasFourToOnePort());
        }

        [TestMethod]
        public void WithThreeToOnePort_HasThreeToOnePort()
        {
            var portList = new PortList(new List<IPort>
            {
                new FourToOnePort(), new ThreeToOnePort()
            });

            Assert.IsTrue(portList.HasThreeToOnePort());
        }

        [TestMethod]
        public void WithSheepPort_HasSheepPort()
        {
            var portList = new PortList(new List<IPort>
            {
                new FourToOnePort(), new SheepPort()
            });

            Assert.IsTrue(portList.HasTwoToOnePort(Sheep.SheepType));
        }
    }
}
