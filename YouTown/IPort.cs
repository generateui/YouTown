﻿using System;

namespace YouTown
{
    /// <summary>
    /// A "trading factor" a player can acquire to reduce cost of trading resources to the bank
    /// </summary>
    /// By default, players all have one 4:1 port
    public interface IPort : IGameItem
    {
        Location WaterLocation { get; }
        Location LandLocation { get; }
        ResourceType ResourceType { get; }
        Edge Edge { get; }
        int InAmount { get; }
        int OutAmount { get; }
        //        int Divide(ResourceList resources, ResourceType resourceType);
        bool CanTrade(IResource resource);
        bool IsRandom { get; }
        Color Color { get; }
        bool HasResource { get; }
        // setLocationAndDirection
    }

    public abstract class PortBase : IPort
    {
        protected PortBase(int id, Location waterLocation, Location landLocation)
        {
            Id = id;
            WaterLocation = waterLocation;
            LandLocation = landLocation;

            Edge = new Edge(waterLocation, landLocation);
        }

        /// <inheritdoc />
        public int Id { get; }

        public Location WaterLocation { get; }
        public Location LandLocation { get; }
        public virtual ResourceType ResourceType { get; }
        public Edge Edge { get; }
        public virtual int InAmount { get; }
        public virtual int OutAmount { get; }
        public virtual bool IsRandom { get; }
        public virtual Color Color { get; }
        public virtual bool HasResource { get; }

        public virtual bool CanTrade(IResource resource)
        {
            throw new NotImplementedException();
        }

        protected bool Equals(PortBase other)
        {
            return Id == other.Id &&
                   Equals(WaterLocation, other.WaterLocation) &&
                   Equals(LandLocation, other.LandLocation) &&
                   Equals(ResourceType, other.ResourceType) &&
                   InAmount == other.InAmount &&
                   OutAmount == other.OutAmount &&
                   IsRandom == other.IsRandom &&
                   Equals(Color, other.Color) &&
                   HasResource == other.HasResource;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PortBase)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
                hashCode = (hashCode * 397) ^ (WaterLocation?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (LandLocation?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (ResourceType?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ InAmount;
                hashCode = (hashCode * 397) ^ OutAmount;
                hashCode = (hashCode * 397) ^ IsRandom.GetHashCode();
                hashCode = (hashCode * 397) ^ (Color?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ HasResource.GetHashCode();
                return hashCode;
            }
        }
    }

    public class FourToOnePort : PortBase
    {
        public FourToOnePort(int id, Location waterLocation, Location landLocation)
            : base(id, waterLocation, landLocation) { }

        public override Color Color => Color.White;
        public override int InAmount => 4;
        public override int OutAmount => 1;

        public override bool CanTrade(IResource resource)
        {
            var resourceType = resource.ResourceType;
            return resourceType.Equals(Wheat.WheatType) ||
                   resourceType.Equals(Ore.OreType) ||
                   resourceType.Equals(Timber.TimberType) ||
                   resourceType.Equals(Sheep.SheepType) ||
                   resourceType.Equals(Clay.ClayType);
        }
    }

    public class ThreeToOnePort : PortBase
    {
        public ThreeToOnePort(int id, Location waterLocation, Location landLocation)
            : base(id, waterLocation, landLocation)
        {
        }

        public override Color Color => Color.White;
        public override int InAmount => 3;
        public override int OutAmount => 1;

        public override bool CanTrade(IResource resource)
        {
            var resourceType = resource.ResourceType;
            return resourceType.Equals(Wheat.WheatType) ||
                   resourceType.Equals(Ore.OreType) ||
                   resourceType.Equals(Timber.TimberType) ||
                   resourceType.Equals(Sheep.SheepType) ||
                   resourceType.Equals(Clay.ClayType);
        }
    }

    public class WheatPort : PortBase
    {
        public WheatPort(int id, Location waterLocation, Location landLocation)
            : base(id, waterLocation, landLocation) { }
        public override int InAmount => 2;
        public override int OutAmount => 1;
        public override ResourceType ResourceType => Wheat.WheatType;
        public override Color Color => ResourceType.Color;
        public override bool CanTrade(IResource resource) => resource.ResourceType.Equals(ResourceType);
    }
    public class ClayPort : PortBase
    {
        public ClayPort(int id, Location waterLocation, Location landLocation)
            : base(id, waterLocation, landLocation) { }
        public override int InAmount => 2;
        public override int OutAmount => 1;
        public override ResourceType ResourceType => Clay.ClayType;
        public override Color Color => ResourceType.Color;
        public override bool CanTrade(IResource resource) => resource.ResourceType.Equals(ResourceType);
    }
    public class TimberPort : PortBase
    {
        public TimberPort(int id, Location waterLocation, Location landLocation)
            : base(id, waterLocation, landLocation) { }
        public override int InAmount => 2;
        public override int OutAmount => 1;
        public override ResourceType ResourceType => Timber.TimberType;
        public override Color Color => ResourceType.Color;
        public override bool CanTrade(IResource resource) => resource.ResourceType.Equals(ResourceType);
    }
    public class OrePort : PortBase
    {
        public OrePort(int id, Location waterLocation, Location landLocation)
            : base(id, waterLocation, landLocation) { }
        public override int InAmount => 2;
        public override int OutAmount => 1;
        public override ResourceType ResourceType => Ore.OreType;
        public override Color Color => ResourceType.Color;
        public override bool CanTrade(IResource resource) => resource.ResourceType.Equals(ResourceType);
    }
    public class SheepPort : PortBase
    {
        public SheepPort(int id, Location waterLocation, Location landLocation)
            : base(id, waterLocation, landLocation) { }
        public override int InAmount => 2;
        public override int OutAmount => 1;
        public override ResourceType ResourceType => Sheep.SheepType;
        public override Color Color => ResourceType.Color;
        public override bool CanTrade(IResource resource) => resource.ResourceType.Equals(ResourceType);
    }
}
