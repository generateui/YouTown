namespace YouTown
{
    /// <summary>
    /// Wraps a string to ensure type safety
    /// </summary>
    /// Tiny type implementation pattern
    public class ResourceType
    {
        private readonly string _resourceType;
        public ResourceType(string resourceType)
        {
            _resourceType = resourceType;
        }

        protected bool Equals(ResourceType other)
        {
            return string.Equals(_resourceType, other._resourceType);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ResourceType) obj);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return _resourceType?.GetHashCode() ?? 0;
        }
    }

    /// <summary>
    /// Value for the player to buy stuff with. 
    /// </summary>
    /// An enum here would restrict us any future implementations. Besides the core 5 resources
    /// many more are available in the YouTown universe such as 
    /// -gold (exchangeable to a basic resource)
    /// -jungle (acts as gold when buying development cards)
    /// -paper (city forest production)
    /// -coin (city mountain production)
    /// -cloth (city pasture production)
    public interface IResource : IGameItem
    {
        bool IsTradeable { get; }
        Color Color { get; }
        ResourceType ResourceType { get; }
    }

    public abstract class ResourceBase : IResource
    {
        protected ResourceBase(int id)
        {
            Id = id;
        }

        /// <inheritdoc/>
        public int Id { get; }
        public virtual bool IsTradeable { get; }
        public virtual Color Color { get; }
        public virtual ResourceType ResourceType { get; }

        protected bool Equals(ResourceBase other)
        {
            return Id == other.Id &&
                IsTradeable == other.IsTradeable && 
                Equals(Color, other.Color) && 
                Equals(ResourceType, other.ResourceType);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ResourceBase) obj);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
                hashCode = (hashCode*397) ^ IsTradeable.GetHashCode();
                hashCode = (hashCode*397) ^ (Color?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ (ResourceType?.GetHashCode() ?? 0);
                return hashCode;
            }
        }
    }

    public class Wheat : ResourceBase
    {
        public static readonly ResourceType WheatType = new ResourceType("wheat");
        public Wheat(int id) : base(id)
        {
        }
        public override bool IsTradeable => true;
        public override Color Color => Color.DarkYellow;
        public override ResourceType ResourceType => WheatType;
    }

    public class Timber : ResourceBase
    {
        public static readonly ResourceType TimberType = new ResourceType("timber");
        public Timber(int id) : base(id)
        {
        }
        public override bool IsTradeable => true;
        public override Color Color => Color.DarkGreen;
        public override ResourceType ResourceType => TimberType;
    }

    public class Clay : ResourceBase
    {
        public static readonly ResourceType ClayType = new ResourceType("clay");
        public Clay(int id) : base(id)
        {
        }
        public override bool IsTradeable => true;
        public override Color Color => Color.Red;
        public override ResourceType ResourceType => ClayType;
    }

    public class Ore : ResourceBase
    {
        public static readonly ResourceType OreType = new ResourceType("ore");
        public Ore(int id) : base(id)
        {
        }
        public override bool IsTradeable => true;
        public override Color Color => Color.Purple;
        public override ResourceType ResourceType => OreType;
    }

    public class Sheep : ResourceBase
    {
        public static readonly ResourceType SheepType = new ResourceType("sheep");
        public Sheep(int id) : base(id)
        {
        }
        public override bool IsTradeable => true;
        public override Color Color => Color.LightGreen;
        public override ResourceType ResourceType => SheepType;
    }
}
