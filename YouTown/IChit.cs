namespace YouTown
{
    /// <summary>
    /// A number on a hexagon tile
    /// </summary>
    /// Represented as simple type exposing some convenience getters. Has a 
    /// <see cref="RandomChit"/> implementation to represent a chit to be 
    /// replaced at board setup.
    public interface IChit : IGameItem
    {
        int Number { get; }
        int Chance { get; }
        bool IsRed { get; }
    }

    public abstract class ChitBase : IChit
    {
        protected ChitBase(int id)
        {
            Id = id;
        }

        /// <inheritdoc/>
        public int Id { get; }
        public virtual int Number { get; }
        public virtual int Chance { get; }
        public virtual bool IsRed { get; }

        protected bool Equals(ChitBase other)
        {
            return Id == other.Id && Number == other.Number;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ChitBase) obj);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                return (Id*397) ^ Number;
            }
        }
    }
    /// <summary>
    /// To be replaced with a real chit when the board is setup
    /// </summary>
    public class RandomChit : ChitBase
    {
        public RandomChit(int id) : base(id) { }
    }
    public class Chit2 : ChitBase
    {
        public Chit2(int id = Identifier.DontCare) : base(id) { }
        public override int Number => 2;
        public override int Chance => 1;
    }
    public class Chit3 : ChitBase
    {
        public Chit3(int id = Identifier.DontCare) : base(id) { }
        public override int Number => 3;
        public override int Chance => 2;
    }
    public class Chit4 : ChitBase
    {
        public Chit4(int id = Identifier.DontCare) : base(id) { }
        public override int Number => 4;
        public override int Chance => 3;
    }
    public class Chit5 : ChitBase
    {
        public Chit5(int id = Identifier.DontCare) : base(id) { }
        public override int Number => 5;
        public override int Chance => 4;
    }
    public class Chit6 : ChitBase
    {
        public Chit6(int id = Identifier.DontCare) : base(id) { }
        public override int Number => 6;
        public override int Chance => 5;
        public override bool IsRed => true;
    }
    public class Chit8 : ChitBase
    {
        public Chit8(int id = Identifier.DontCare) : base(id) { }
        public override int Number => 8;
        public override int Chance => 5;
        public override bool IsRed => true;
    }
    public class Chit9 : ChitBase
    {
        public Chit9(int id = Identifier.DontCare) : base(id) { }
        public override int Number => 9;
        public override int Chance => 4;
    }
    public class Chit10 : ChitBase
    {
        public Chit10(int id = Identifier.DontCare) : base(id) { }
        public override int Number => 10;
        public override int Chance => 3;
    }
    public class Chit11 : ChitBase
    {
        public Chit11(int id = Identifier.DontCare) : base(id) { }
        public override int Number => 11;
        public override int Chance => 2;
    }
    public class Chit12 : ChitBase
    {
        public Chit12(int id = Identifier.DontCare) : base(id) { }
        public override int Number => 12;
        public override int Chance => 1;
    }
}
