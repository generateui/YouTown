using System.Collections.Generic;
using System.Linq;

namespace YouTown
{
    public class TradeOffer : IGameItem
    {
        public TradeOffer(
            int id,
            IPlayer player, 
            IResourceList offered, 
            IResourceList requested)
        {
            Id = id;
            Player = player;
            Offered = offered;
            Requested = requested;
            Responses = new List<ITradeOfferResponse>();
        }

        public TradeOffer(TradeOfferData data, IRepository repo)
        {
            Id = data.Id;
            Player = repo.Get<IPlayer>(data.PlayerId);
            Offered = data.Offered.FromData();
            Requested = data.Requested.FromData();
            Responses = data.Responses.Select(r => r.FromData(repo)).ToList();
        }

        public int Id { get; }
        public IPlayer Player { get; }
        public IResourceList Offered { get; }
        public IResourceList Requested { get; }
        public IList<ITradeOfferResponse> Responses { get; }

        public TradeOfferData ToData() =>
            new TradeOfferData
            {
                Id = Id,
                PlayerId = Player.Id,
                Offered = Offered.ToData(),
                Requested = Requested.ToData(),
                Responses = Responses.Select(r => r.ToData()).ToList()
            };
    }

    public interface ITradeOfferResponse : IGameItem
    {
        IPlayer Player { get; }
        IResourceList CounterOffered { get; }
        IResourceList CounterRequested { get; }
        bool IsAccepted { get; }
        bool IsRejected { get; }
        bool IsCounterOffered { get; }
    }

    public class Accept : ITradeOfferResponse
    {
        public Accept(int id, IPlayer player)
        {
            Player = player;
            Id = id;
        }

        public int Id { get; }
        public IPlayer Player { get; }
        public IResourceList CounterOffered => null;
        public IResourceList CounterRequested => null;
        public bool IsAccepted => true;
        public bool IsRejected => false;
        public bool IsCounterOffered => false;
    }

    public class Reject : ITradeOfferResponse
    {
        public Reject(int id, IPlayer player)
        {
            Player = player;
            Id = id;
        }

        public int Id { get; }
        public IPlayer Player { get; }
        public IResourceList CounterOffered => null;
        public IResourceList CounterRequested => null;
        public bool IsAccepted => false;
        public bool IsRejected => true;
        public bool IsCounterOffered => false;
    }

    public class CounterOffer : ITradeOfferResponse
    {
        public CounterOffer(int id, IPlayer player, IResourceList counterOffered, IResourceList counterRequested)
        {
            Player = player;
            CounterOffered = counterOffered;
            CounterRequested = counterRequested;
            Id = id;
        }

        public int Id { get; }
        public IPlayer Player { get; }
        public IResourceList CounterOffered { get; }
        public IResourceList CounterRequested { get; }
        public bool IsAccepted => false;
        public bool IsRejected => false;
        public bool IsCounterOffered => true;
    }
}
