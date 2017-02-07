using System.Collections.Generic;

namespace YouTown
{
    public class Produce
    {
        public Produce(IProducer producer, IHex hex, IResourceList resources)
        {
            Producer = producer;
            Hex = hex;
            Resources = resources;
        }

        public IProducer Producer { get;}
        public IHex Hex { get; }
        public IResourceList Resources { get; }
    }

    public class ProductionShortage
    {
        public ProductionShortage(IPlayer player, ResourceType resourceType, int amountShort)
        {
            Player = player;
            ResourceType = resourceType;
            AmountShort = amountShort;
        }

        public IPlayer Player { get; }
        public ResourceType ResourceType { get; }
        public int AmountShort { get; }
    }

    public class Production
    {
        public Production(
            IReadOnlyDictionary<IPlayer, IResourceList> toDistribute, 
            IEnumerable<Produce> all, 
            IEnumerable<ProductionShortage> shortages)
        {
            ToDistribute = toDistribute;
            All = all;
            Shortages = shortages;
        }

        public IReadOnlyDictionary<IPlayer, IResourceList> ToDistribute { get; }
        public IEnumerable<Produce> All { get; }
        public IEnumerable<ProductionShortage> Shortages { get; }

    }
}
