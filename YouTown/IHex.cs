using System;

namespace YouTown
{
    /// <summary>
    /// 
    /// </summary>
    public class HexType
    {
        private string _hexType;

        public HexType(string hexType)
        {
            _hexType = hexType;
        }

        public string Value => _hexType;
    }

    /// <summary>
    /// A tile on a location on the board optionally producing resources
    /// </summary>
    /// Hexagonal shape of a tile positioned at a location. An IHex may have many forms
    /// beside the core 7 ones (5 plus desert & water). Examples:
    /// -Volcano producing gold. When producing, a dice is rolled. If a town matches 
    /// the number of the dice, it explodes and is removed from board. Cities becomes towns
    /// when hit.
    /// -Jungle producing tokes acting as gold when buying development cards
    /// -Random represents a "real" hex when designing a board
    /// -None represents the border of a board for nice graphical reendering purposes
    public interface IHex : IGameItem
    {
        Location Location { get; }
        Color Color { get; }
        HexType HexType { get; }
        bool CanHaveChit { get; }
        bool IsPartOfGame { get; }
        bool CanHaveRobber { get; }
        bool CanHavePirate { get; }
        bool CanHavePort { get; }
        bool IsRandom { get; }
        bool ProducesResource { get; }

        /// Mutable as gameplays exist where chits can be moved, removed and discovered
        IChit Chit { get; set; }
        IPort Port { get; set; }
        IResource Produce();

        /// <summary>
        /// Required to replace randomhexes with whatever hex is given
        /// </summary>
        /// Simple setters would suffice, but I want the API to be as immutable and 
        /// functional as possible.
        /// <param name="id">id for the newly cloned IHex instance</param>
        /// <param name="location">location of the newly cloned IHex instance</param>
        /// <returns>new instance of concrete IHex type</returns>
        IHex Clone(int id, Location location);
    }
    public class HexBase : IHex
    {
        public HexBase(int id, Location location, IPort port)
        {
            Location = location;
            Id = id;
            Port = port;
        }

        /// <inheritdoc/>
        public int Id { get; }
        public Location Location { get; }
        public virtual Color Color { get; }

        public virtual HexType HexType
        {
            get { throw new NotImplementedException(); }
        }

        public virtual bool CanHaveChit { get; }
        public virtual bool IsPartOfGame { get; }
        public virtual bool CanHaveRobber { get; }
        public virtual bool CanHavePirate { get; }
        public virtual bool CanHavePort { get; }
        public virtual bool IsRandom { get; }
        public virtual bool ProducesResource { get; }
        public IChit Chit { get; set; }

        public IPort Port { get; set; }

        public virtual IResource Produce()
        {
            throw new NotImplementedException();
        }

        public virtual IHex Clone(int id, Location location)
        {
            throw new NotImplementedException();
        }
    }

    public class RandomHex : HexBase
    {
        public RandomHex(int id = Identifier.DontCare, Location location = null, IPort port = null) : base(id, location, port) { }
        public override bool IsRandom => true;
        public override Color Color => Color.Black;
    }

    public class Water : HexBase
    {
        public static readonly HexType WaterType = new HexType("water");
        public Water(int id = Identifier.DontCare, Location location = null, IPort port = null) : base(id, location, port) { }
        public override Color Color => Color.Blue;
        public override bool CanHavePirate => true;
        public override bool CanHavePort => true;
        public override HexType HexType => WaterType;
        public override bool IsPartOfGame => true;
        public override IHex Clone(int id, Location location) => new Water(id, location, Port);
    }

    public class Desert : HexBase
    {
        public static readonly HexType DesertType = new HexType("desert");
        public Desert(int id = Identifier.DontCare, Location location = null) : base(id, location, null) { }
        public override Color Color => Color.LightYellow;
        public override bool CanHaveRobber => true;
        public override HexType HexType => DesertType;
        public override IHex Clone(int id, Location location) => new Desert(id, location);
    }

    public class Pasture : HexBase
    {
        public static readonly HexType PastureType = new HexType("pasture");
        public Pasture(int id = Identifier.DontCare, Location location = null) : base(id, location, null) { }
        public override HexType HexType => PastureType;
        public override Color Color => Color.LightGreen;
        public override bool CanHaveRobber => true;
        public override bool IsPartOfGame => true;
        public override bool CanHaveChit => true;
        public override bool ProducesResource => true;
        public override IHex Clone(int id, Location location) => new Pasture(id, location);
    }

    public class Forest : HexBase
    {
        public static readonly HexType ForestType = new HexType("forest");
        public Forest(int id = Identifier.DontCare, Location location = null) : base(id, location, null) { }
        public override HexType HexType => ForestType;
        public override Color Color => Color.DarkGreen;
        public override bool CanHaveRobber => true;
        public override bool IsPartOfGame => true;
        public override bool CanHaveChit => true;
        public override bool ProducesResource => true;
        public override IHex Clone(int id, Location location) => new Forest(id, location);
    }

    public class Hill : HexBase
    {
        public static readonly HexType HillType = new HexType("hill");
        public Hill(int id = Identifier.DontCare, Location location = null) : base(id, location, null) { }
        public override HexType HexType => HillType;
        public override Color Color => Color.Red;
        public override bool CanHaveRobber => true;
        public override bool IsPartOfGame => true;
        public override bool CanHaveChit => true;
        public override bool ProducesResource => true;
        public override IHex Clone(int id, Location location) => new Hill(id, location);
    }

    public class Mountain : HexBase
    {
        public static readonly HexType MountainType = new HexType("mountain");
        public Mountain(int id = Identifier.DontCare, Location location = null) : base(id, location, null) { }
        public override HexType HexType => MountainType;
        public override Color Color => Color.LightGreen;
        public override bool CanHaveRobber => true;
        public override bool IsPartOfGame => true;
        public override bool CanHaveChit => true;
        public override bool ProducesResource => true;
        public override IHex Clone(int id, Location location) => new Mountain(id, location);
    }

    public class Field : HexBase
    {
        public static readonly HexType FieldType = new HexType("field");
        public Field(int id = Identifier.DontCare, Location location = null) : base(id, location, null) { }
        public override HexType HexType => FieldType;
        public override Color Color => Color.DarkYellow;
        public override bool CanHaveRobber => true;
        public override bool IsPartOfGame => true;
        public override bool CanHaveChit => true;
        public override bool ProducesResource => true;
        public override IHex Clone(int id, Location location) => new Field(id, location);
    }
}
