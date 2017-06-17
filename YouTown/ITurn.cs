using System.Collections.Generic;

namespace YouTown
{
    public interface ITurn : IGameItem
    {
        /// <summary>
        /// Consecutive 1-based number of the turn within a game
        /// </summary>
        int Number { get; }
        IPlayer Player { get; }
    }

    public interface IPlayTurnsTurn : ITurn, IGameItem
    {
        bool HasPlayedDevelopmentCard { get; set; }
        IList<TradeOffer> TradeOffers { get; }
    }

    public class PlaceTurn : ITurn
    {
        public PlaceTurn(int id, int number, IPlayer player)
        {
            Id = id;
            Number = number;
            Player = player;
        }

        public PlaceTurn(PlaceTurnData data, IRepository repo)
        {
            Id = data.Id;
            Number = data.Number;
            Player = repo.Get<IPlayer>(data.PlayerId);
        }

        public int Id { get; }
        public int Number { get; }
        public IPlayer Player { get; }

        public PlaceTurnData ToData() =>
            new PlaceTurnData
            {
                Id = Id,
                PlayerId = Player.Id,
                Number = Number,
            };
    }

    public class PlayTurnsTurn : IPlayTurnsTurn
    {
        public PlayTurnsTurn(int id, int number, IPlayer player, List<TradeOffer> tradeOffers = null)
        {
            Number = number;
            Player = player;
            Id = id;
            TradeOffers = tradeOffers ?? new List<TradeOffer>();
        }

        public int Id { get; }
        public int Number { get; }
        public IPlayer Player { get; }
        public bool HasPlayedDevelopmentCard { get; set; }
        public IList<TradeOffer> TradeOffers { get; } 

        protected bool Equals(PlayTurnsTurn other)
        {
            return Number == other.Number && HasPlayedDevelopmentCard;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PlayTurnsTurn) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return Number ^ HasPlayedDevelopmentCard.GetHashCode();
            }
        }
    }
}
