using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YouTown.UnitTest
{
    [TestClass]
    public class ResourceListTest
    {
        [TestMethod]
        public void HalfCountOf8_Is4()
        {
            var resourceList = new ResourceList(new List<IResource>
            {
                new Wheat(),new Wheat(),new Wheat(),new Wheat(),
                new Wheat(),new Wheat(),new Wheat(),new Wheat(),
            });

            Assert.AreEqual(4, resourceList.HalfCount());
        }

        [TestMethod]
        public void HalfCountOf9_Is5()
        {
            var resourceList = new ResourceList(new List<IResource>
            {
                new Wheat(),new Wheat(),new Wheat(),new Wheat(),new Wheat(),
                new Wheat(),new Wheat(),new Wheat(),new Wheat(),
            });

            Assert.AreEqual(5, resourceList.HalfCount());
        }

        [TestMethod]
        public void IterationOfNormalResources_YieldsInCorrectOrder()
        {
            var resourceList = new ResourceList(new List<IResource>
            {
                new Wheat(), new Timber(), new Clay(), new Ore(), new Sheep()
            });
            var expectedResourceTypeOrder = ResourceList.ResourceTypeOrder;
            int i = 0;
            foreach (var resource in resourceList)
            {
                ResourceType expectedResourceType = expectedResourceTypeOrder[i];
                Assert.AreEqual(expectedResourceType, resource.ResourceType);
                i++;
            }
        }

        private class DerpyResource : IResource
        {
            public static readonly ResourceType Derpy = new ResourceType("derpy", Color.Black);
            public int Id { get; }
            public bool IsTradeable { get; }
            public ResourceType ResourceType => Derpy;
        }

        [TestMethod]
        public void IterationWithForeignResource_YieldsInCorrectOrder()
        {
            var resourceList = new ResourceList(new List<IResource>
            {
                new DerpyResource(), new Wheat(), new Timber(), new Clay(), new Ore(), new Sheep()
            });
            var expectedResourceTypeOrder = ResourceList.ResourceTypeOrder;
            expectedResourceTypeOrder.Add(DerpyResource.Derpy); // unknown types come last
            int i = 0;
            foreach (var resource in resourceList)
            {
                ResourceType expectedResourceType = expectedResourceTypeOrder[i];
                Assert.AreEqual(expectedResourceType, resource.ResourceType);
                i++;
            }
        }

        [TestMethod]
        public void EqualResourceLists_AreEqual()
        {
            var resourceList1 = new ResourceList(new List<IResource>
            {
                new Wheat(), new Timber(), new Clay(), new Ore(), new Sheep()
            });
            var resourceList2 = new ResourceList(new List<IResource>
            {
                new Wheat(), new Timber(), new Clay(), new Ore(), new Sheep()
            });

            Assert.IsTrue(resourceList1.EqualsResources(resourceList2));
        }

        [TestMethod]
        public void NotEqualResourceLists_AreEqual()
        {
            var resourceList1 = new ResourceList(new List<IResource>
            {
                new Wheat(), new Timber(), new Clay(), new Ore(), new Sheep()
            });
            var resourceList2 = new ResourceList(new List<IResource>
            {
                new Wheat(), new Timber(), new Clay(), new Ore()
            });

            Assert.IsFalse(resourceList1.EqualsResources(resourceList2));
        }

        [TestMethod]
        public void DoesNotHaveOther_DoesNotHaveAtLeast()
        {
            var resourceList1 = new ResourceList(new List<IResource>
            {
                new Wheat(), new Timber(), new Clay(), new Ore(), new Sheep()
            });
            var resourceList2 = new ResourceList(new List<IResource>
            {
                new Wheat(), new Timber(), new Clay(), new Ore()
            });

            Assert.IsFalse(resourceList2.HasAtLeast(resourceList1));
        }

        [TestMethod]
        public void DoesHaveOther_HasAtLeast()
        {
            var resourceList1 = new ResourceList(new List<IResource>
            {
                new Wheat(), new Timber(), new Clay(), new Ore(), new Sheep()
            });
            var resourceList2 = new ResourceList(new List<IResource>
            {
                new Wheat(), new Timber(), new Clay(), new Ore()
            });

            Assert.IsTrue(resourceList1.HasAtLeast(resourceList2));
        }
    }
}
