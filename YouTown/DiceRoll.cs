namespace YouTown
{
    public class DiceRoll
    {
        public DiceRoll(int die1, int die2)
        {
            Die1 = die1;
            Die2 = die2;
        }

        public int Die1 { get; }
        public int Die2 { get; }
        public int Sum => Die1 + Die2;

        protected bool Equals(DiceRoll other)
        {
            return Die1 == other.Die1 && Die2 == other.Die2;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DiceRoll) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (Die1*397) ^ Die2;
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"Die1: {Die1}, Die2: {Die2}";
        }
    }
}
