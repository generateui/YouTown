using System.Collections.Generic;

namespace YouTown
{
    public interface IPortList
    {
        IList<IPort> Ports { get; }
    }
    public class PortList : IPortList
    {
        public PortList(IList<IPort> ports)
        {
            Ports = ports;
        }

        public IList<IPort> Ports { get; }
    }

}