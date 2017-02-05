using System.Collections.Generic;

namespace YouTown
{
    public interface IBank
    {
        IResourceList Resources { get; }
        IList<IDevelopmentCard> DevelopmentCards { get; }
    }

    public class Bank : IBank
    {
        public Bank(IResourceList resources, IList<IDevelopmentCard> developmentCards)
        {
            Resources = resources;
            DevelopmentCards = developmentCards;
        }

        public IResourceList Resources { get; }
        public IList<IDevelopmentCard> DevelopmentCards { get; }
    }
}
