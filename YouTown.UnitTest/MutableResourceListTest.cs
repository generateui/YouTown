using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YouTown.UnitTest
{
    [TestClass]
    public class MutableResourceListTest
    {
        [TestMethod]
        public void AddResource_AddsResource()
        {
            var resourceList = new ResourceList(new List<IResource>
            {
                new Wheat(), new Clay(), new Ore(), new Sheep()
            });
            var mutableResourceList = new MutableResourceList();
            mutableResourceList.Add(new Wheat());
            mutableResourceList.Add(new Clay());
            mutableResourceList.Add(new Ore());
            mutableResourceList.Add(new Sheep());

            Assert.IsTrue(mutableResourceList.HasAtLeast(resourceList));
        }

        [TestMethod]
        public void RemoveResource_RemovesResource()
        {
            var resourceList = new ResourceList(new List<IResource>
            {
                new Wheat(), new Clay(), new Ore(), new Sheep()
            });
            var mutableResourceList = new MutableResourceList();
            mutableResourceList.Add(new Wheat());
            mutableResourceList.Add(new Clay());
            mutableResourceList.Add(new Ore());

            Assert.AreEqual(3, mutableResourceList.Count);
            Assert.AreEqual(3, mutableResourceList.ResourceTypes.Count());

            var sheep = new Sheep();
            mutableResourceList.Add(sheep);
            Assert.AreEqual(4, mutableResourceList.Count);
            Assert.AreEqual(4, mutableResourceList.ResourceTypes.Count());

            mutableResourceList.Remove(sheep);
            Assert.AreEqual(3, mutableResourceList.Count);
            Assert.AreEqual(3, mutableResourceList.ResourceTypes.Count());
            Assert.IsFalse(mutableResourceList.HasAtLeast(resourceList));

            mutableResourceList.Add(sheep);
            Assert.IsTrue(mutableResourceList.HasAtLeast(resourceList));
            Assert.AreEqual(4, mutableResourceList.Count);
            Assert.AreEqual(4, mutableResourceList.ResourceTypes.Count());
        }
    }
}
