using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace YouTown
{
    /// <summary>
    /// List of resource where resources are grouped by <see cref="ResourceType"/>
    /// </summary>
    /// When iterating over a resourcelist, it yields <see cref="IResource"/> instances
    /// in semi-determinate order of timber, wheat, sheep, clay, ore, other (indeterminate).
    /// TODO: Have grouporder extensible
    public interface IResourceList : IReadOnlyList<IResource>
    {
        bool HasType(ResourceType resourceType);
        IResourceList OfType(ResourceType resourceType);
        /// <summary>
        /// Currently indeterminate order 
        /// </summary>
        /// TODO: should be have this conform to resourcetype order as well?
        IEnumerable<ResourceType> ResourceTypes { get; }
        string ToSummary();
        int HalfCount(); // TODO: pass in IHalfCountStrategy
        bool HasAtLeast(IResourceList what);
    }

    public class ResourceList : IResourceList
    {
        public static ResourceList Empty = new ResourceList();
        public static readonly List<ResourceType> ResourceTypeOrder = new List<ResourceType>
        {
            Timber.TimberType, Wheat.WheatType, Sheep.SheepType, Clay.ClayType, Ore.OreType
        };

        protected readonly Dictionary<ResourceType, List<IResource>> _resources = 
            new Dictionary<ResourceType, List<IResource>>();

        public ResourceList(IEnumerable<IResource> list)
        {
            AddRangeSafe(list);
        }

        public ResourceList() { }

        public ResourceList(IResource resource)
        {
            AddSafe(resource);
        }

        protected void AddSafe(IResource resource)
        {
            var resourceType = resource.ResourceType;
            if (!_resources.ContainsKey(resourceType))
            {
                _resources[resourceType] = new List<IResource>();
            }
            _resources[resourceType].Add(resource);
        }

        protected void AddRangeSafe(params IResource[] resources)
        {
            AddRangeSafe((IEnumerable<IResource>)resources);
        }

        protected void AddRangeSafe(IEnumerable<IResource> resources)
        {
            foreach (var resource in resources)
            {
                AddSafe(resource);
            }
        }

        public bool HasType(ResourceType resourceType)
        {
            return _resources.ContainsKey(resourceType);
        }

        public IResourceList OfType(ResourceType resourceType)
        {
            if (_resources.ContainsKey(resourceType))
            {
                return new ResourceList(_resources[resourceType]);
            }
            return Empty;
        }

        public IEnumerable<ResourceType> ResourceTypes => _resources
            .Where(kvp => kvp.Value.Any())
            .Select(kvp => kvp.Key);

        public string ToSummary()
        {
            var summaries = new List<string>();
            foreach (var resourceType in _resources.Keys)
            {
                if (!HasType(resourceType))
                {
                    continue;
                }
                int count = _resources[resourceType].Count;
                summaries.Add($"{count} {resourceType.Value}");
            }
            return string.Join(", ", summaries);
        }

        public int HalfCount()
        {
            var count = _resources.SelectMany(kvp => kvp.Value).Count();
            if (count%2 != 0)
            {
                count++;
            }
            return count/2;
        }

        public bool HasAtLeast(IResourceList what)
        {
            foreach (var resourceType in what.ResourceTypes)
            {
                if (!HasType(resourceType))
                {
                    return false;
                }
                if (_resources[resourceType].Count < what.OfType(resourceType).Count())
                {
                    return false;
                }
            }
            return true;
        }

        private IEnumerable<IResource> CreateEnumerator()
        {
            var unorderedTypes = _resources.Keys
                .Where(rt => !ResourceTypeOrder.Contains(rt))
                .ToList();
            var order = ResourceTypeOrder
                .ToList()
                .Concat(unorderedTypes);
            foreach (var resourceType in order)
            {
                if (!_resources.ContainsKey(resourceType))
                {
                    continue;
                }
                var resourcesOfType = _resources[resourceType];
                foreach (var resource in resourcesOfType)
                {
                    yield return resource;
                }
            }
        }

        /// <inheritdoc />
        public IEnumerator<IResource> GetEnumerator()
        {
            return CreateEnumerator().GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return CreateEnumerator().GetEnumerator();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return ToSummary();
        }

        /// <summary>
        /// True if total count and count per <see cref="ResourceTypes"/> are equal
        /// </summary>
        /// TODO: bump to interface?
        /// <returns></returns>
        public bool EqualsResources(ResourceList other)
        {
            if (Count != other.Count)
            {
                return false;
            }
            foreach (var resourceType in _resources.Keys)
            {
                if (HasType(resourceType) && !other.HasType(resourceType))
                {
                    return false;
                }
                if (!HasType(resourceType) && other.HasType(resourceType))
                {
                    return false;
                }
                if (HasType(resourceType) && other.HasType(resourceType))
                {
                    if (OfType(resourceType).Count() != other.OfType(resourceType).Count())
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <inheritdoc />
        public int Count => _resources.SelectMany(kvp => kvp.Value).Count();

        /// <inheritdoc />
        /// TODO: this is horribly inefficient given some call patterns
        public IResource this[int index] => _resources.SelectMany(kvp => kvp.Value).ToList()[index];
    }

    public class MutableResourceList : ResourceList, IList<IResource>, IResourceList
    {
        /// <inheritdoc />
        public void Add(IResource resource)
        {
            AddSafe(resource);
        }

        /// <inheritdoc />
        public void Clear()
        {
            _resources.Clear();
        }

        /// <inheritdoc />
        public bool Contains(IResource resource)
        {
            if (!HasType(resource.ResourceType))
            {
                return false;
            }
            return OfType(resource.ResourceType).Any(r => r.Equals(resource));
        }

        /// <inheritdoc />
        public void CopyTo(IResource[] array, int arrayIndex)
        {
            this.ToArray().CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public bool Remove(IResource resource)
        {
            if (!HasType(resource.ResourceType))
            {
                return false;
            }
            return _resources[resource.ResourceType].Remove(resource);
        }

        /// <inheritdoc />
        public bool IsReadOnly => false;

        /// <inheritdoc />
        public int IndexOf(IResource resource)
        {
            return this.ToList().IndexOf(resource);
        }

        /// <inheritdoc />
        public void Insert(int index, IResource item)
        {
            throw new NotSupportedException("makes no sense");
        }

        /// <inheritdoc />
        public void RemoveAt(int index)
        {
            var list = this.ToList();
            if (index < 0 || index > list.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            var resourceToRemove = this.ToList()[index];
            Remove(resourceToRemove);
        }

        /// <inheritdoc />
        public new IResource this[int index]
        {
            get
            {
                var list = this.ToList();
                if (index < 0 || index > list.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
                var resource = this.ToList()[index];
                return resource;
            }
            set { throw new NotSupportedException("makes no sense"); }
        }
    }
}
