using System.Collections.Generic;
using System.Linq;

namespace YouTown
{
    public interface IPortList
    {
        IList<IPort> Ports { get; }
        bool HasFourToOnePort();
        bool HasThreeToOnePort();
        bool HasTwoToOnePort(ResourceType resourceType);
        int AmountGold(IResourceList resourceList);
        IPort BestPortForResourceType(ResourceType resourceType);
    }
    public class PortList : IPortList
    {
        public PortList(IList<IPort> ports)
        {
            Ports = ports;
        }

        public IList<IPort> Ports { get; }

        public bool HasFourToOnePort() => Ports.Any(p => p.InAmount == 4 && p.OutAmount == 1);

        public bool HasThreeToOnePort() => Ports.Any(p => p.InAmount == 3 && p.OutAmount == 1);

        public bool HasTwoToOnePort(ResourceType resourceType) => Ports.Any(
                p => p.ResourceType != null && 
                p.ResourceType.Equals(resourceType) && 
                p.InAmount == 2 && 
                p.OutAmount == 1);

        public int AmountGold(IResourceList resourceList)
        {
            int total = 0;
            foreach (ResourceType resourceType in resourceList.ResourceTypes)
            {
                var resourcesOfType = resourceList.OfType(resourceType);
                if (!resourcesOfType.Any())
                {
                    continue;
                }
                IPort port = BestPortForResourceType(resourceType);
                total += port.Divide(resourcesOfType, resourceType);
            }
            return total;
        }

        public IPort BestPortForResourceType(ResourceType resourceType)
        {
            IPort best = Ports.FirstOrDefault();
            foreach (var port in Ports)
            {
                bool canTrade = port.CanTrade(resourceType);
                if (!canTrade)
                {
                    continue;
                }
                bool isBetterDeal = port.InAmount < best.InAmount;
                if (isBetterDeal)
                {
                    best = port;
                }
            }
            return best;
        }
    }

}