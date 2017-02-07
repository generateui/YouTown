using System.Collections.Generic;

namespace YouTown
{
    public interface ITurn : IGameItem
    {
        /// <summary>
        /// Consecutive 1-based number of the turn within a game
        /// </summary>
        int Number { get; }
        //Player Player { get; }
        //List<ITrade> Trades { get; }
    }

    public interface IPlayTurnsTurn : ITurn
    {
        bool HasPlayedDevelopmentCard { get; set; }
    }

    public class PlaceTurn : ITurn
    {
        public PlaceTurn(int id, int number)
        {
            Id = id;
            Number = number;
        }

        public int Id { get; }
        public int Number { get; }
    }

    public class Turn : ITurn
    {
        /// <summary>
        /// Within the scope of a game an int is more then sufficient to identify an item
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Consecutive 1-based number of the turn within a game
        /// </summary>
        public int Number { get; }

        protected bool Equals(Turn other)
        {
            return Id == other.Id && Number == other.Number;
        }

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Turn) obj);
        }

        /// <summary>Serves as the default hash function. </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (Id*397) ^ Number;
            }
        }
    }
}
