using System.Collections.Generic;
using System.Linq;

namespace YouTown
{
    /// <summary>
    /// Resources produced at a producer on a hex
    /// </summary>
    public class Produce
    {
        public Produce(IProducer producer, IHex hex, IResourceList resources)
        {
            Producer = producer;
            Hex = hex;
            Resources = resources;
        }

        public Produce(ProduceData data, IRepository repo)
        {
            Producer = repo.Get<IProducer>(data.ProducerId);
            Hex = repo.Get<IHex>(data.HexId);
            Resources = data.Resources.FromData();
        }

        public IProducer Producer { get; }
        public IHex Hex { get; }
        public IResourceList Resources { get; }

        public ProduceData ToData() =>
            new ProduceData
            {
                HexId = Hex.Id,
                ProducerId = Producer.Id,
                Resources = Resources.ToData()
            };
    }

    /// <summary>
    /// Bank does not have enough resources to distribute among players
    /// </summary>
    /// When the bank is out of resources, players should not actually get any 
    /// while they have to right to them. The bank is empty, so tough luck.
    /// To enable nice communication off this to players, we want to know what
    /// is exactly not given to the user.
    public class ProductionShortage
    {
        public ProductionShortage(IPlayer player, ResourceType resourceType, int amountShort)
        {
            Player = player;
            ResourceType = resourceType;
            AmountShort = amountShort;
        }
        public ProductionShortage(ProductionShortageData data, IRepository repo)
        {
            Player = repo.Get<IPlayer>(data.PlayerId);
            ResourceType = ResourceType.Parse(data.ResourceType);
            AmountShort = data.AmountShort;
        }

        public IPlayer Player { get; }
        public ResourceType ResourceType { get; }
        public int AmountShort { get; }

        public ProductionShortageData ToData() =>
            new ProductionShortageData
            {
                PlayerId = Player.Id,
                ResourceType = ResourceType.Value,
                AmountShort = AmountShort
            };
    }

    /// <summary>
    /// Summary of every production or -shortage happening while rolling dice
    /// </summary>
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

        public Production(ProductionData data, IRepository repo)
        {
            ToDistribute = data.ToDistribute.ToDictionary(
                kvp => repo.Get<IPlayer>(kvp.Key),
                kvp => kvp.Value.FromData());
            All = data.All.Select(p => new Produce(p, repo)).ToList();
            Shortages = data.Shortages.Select(s => new ProductionShortage(s, repo)).ToList();
        }

        /// <summary>
        /// Resources player will actually get
        /// </summary>
        public IReadOnlyDictionary<IPlayer, IResourceList> ToDistribute { get; }

        /// <summary>
        /// All production, also included production the player may not get (because bank shortage)
        /// </summary>
        public IEnumerable<Produce> All { get; }

        /// <summary>
        /// All shortages within this production run (diceroll).
        /// </summary>
        /// Use this to obtain info to help player understand what's going on
        public IEnumerable<ProductionShortage> Shortages { get; }

        public ProductionData ToData() =>
            new ProductionData
            {
                ToDistribute = ToDistribute.ToDictionary(
                    x => x.Key.Id, 
                    x => x.Value.ToData()),
                All = All.Select(p => p.ToData()).ToList(),
                Shortages = Shortages.Select(s => s.ToData()).ToList()
            };

    }
}
