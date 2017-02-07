﻿using System.Collections.Generic;

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

    public class PlayTurnsTurn : IPlayTurnsTurn
    {
        public PlayTurnsTurn(int id, int number)
        {
            Id = id;
            Number = number;
        }

        public int Id { get; }
        public int Number { get; }
        public bool HasPlayedDevelopmentCard { get; set; }

        protected bool Equals(PlayTurnsTurn other)
        {
            return Id == other.Id && Number == other.Number && HasPlayedDevelopmentCard;
        }

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PlayTurnsTurn) obj);
        }

        /// <summary>Serves as the default hash function. </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (Id*397) ^ Number ^ HasPlayedDevelopmentCard.GetHashCode();
            }
        }
    }
}
