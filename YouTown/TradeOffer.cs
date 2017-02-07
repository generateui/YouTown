using System.Collections.Generic;

namespace YouTown
{
    public class TradeOffer
    {
        public TradeOffer(IPlayer player, IResourceList offered, IResourceList requested)
        {
            Player = player;
            Offered = offered;
            Requested = requested;
            Responses = new List<ITradeOfferResponse>();
        }

        public IPlayer Player { get; }
        public IResourceList Offered { get; }
        public IResourceList Requested { get; }
        public IList<ITradeOfferResponse> Responses { get; }
    }

    public interface ITradeOfferResponse
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
        public Accept(IPlayer player)
        {
            Player = player;
        }

        public IPlayer Player { get; }
        public IResourceList CounterOffered => null;
        public IResourceList CounterRequested => null;
        public bool IsAccepted => true;
        public bool IsRejected => false;
        public bool IsCounterOffered => false;

    }

    public class Reject : ITradeOfferResponse
    {
        public Reject(IPlayer player)
        {
            Player = player;
        }

        public IPlayer Player { get; }
        public IResourceList CounterOffered => null;
        public IResourceList CounterRequested => null;
        public bool IsAccepted => false;
        public bool IsRejected => true;
        public bool IsCounterOffered => false;
    }

    public class CounterOffer : ITradeOfferResponse
    {
        public CounterOffer(IPlayer player, IResourceList counterOffered, IResourceList counterRequested)
        {
            Player = player;
            CounterOffered = counterOffered;
            CounterRequested = counterRequested;
        }

        public IPlayer Player { get; }
        public IResourceList CounterOffered { get; }
        public IResourceList CounterRequested { get; }
        public bool IsAccepted => false;
        public bool IsRejected => false;
        public bool IsCounterOffered => true;
    }
}
