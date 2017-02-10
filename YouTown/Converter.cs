using System;
using System.Linq;

namespace YouTown
{
    /// <summary>
    /// Performs conversion between bond data classes and domain classes
    /// </summary>
    /// To ensure we don't end up with a data layer entangled in the domain layer, 
    /// both are independently specified. This class ensures that we can serialize
    /// domain instances and revive domain instances from data.
    public class Converter
    {
        public LocationData ToLocationData(Location location) => new LocationData
           {
               X = location.X,
               Y = location.Y,
               Z = location.Z
           }; 

        public Location FromData(LocationData data) =>
            new Location(data.X, data.Y, data.Z);

        public EdgeData ToEdgeData(Edge edge) =>
            new EdgeData
            {
                Location1 = ToLocationData(edge.Location1),
                Location2 = ToLocationData(edge.Location2),
            };

        public Edge FromData(EdgeData data) =>
            new Edge(FromData(data.Location1), FromData(data.Location2));

        public Vertex FromData(VertexData data) =>
            new Vertex(FromData(data.Location1), FromData(data.Location2), FromData(data.Location3));

        public VertexData ToVertexData(Vertex vertex) =>
            new VertexData
            {
                Location1 = ToLocationData(vertex.Location1),
                Location2 = ToLocationData(vertex.Location2),
                Location3 = ToLocationData(vertex.Location3),
            };

        public ResourceData ToResourceData(IResource resource)
        {
            ResourceTypeData type = ResourceTypeData.Timber;
            if (resource is Timber) type = ResourceTypeData.Timber;
            if (resource is Wheat) type = ResourceTypeData.Wheat;
            if (resource is Sheep) type = ResourceTypeData.Sheep;
            if (resource is Clay) type = ResourceTypeData.Clay;
            if (resource is Ore) type = ResourceTypeData.Ore;
            if (resource is DummyResource) type = ResourceTypeData.Dummy;
            return new ResourceData
            {
                Id = resource.Id,
                ResourceType = type
            };
        }

        public IResource FromData(ResourceData data)
        {
            switch (data.ResourceType)
            {
                case ResourceTypeData.Timber: return new Timber(data.Id);
                case ResourceTypeData.Wheat: return new Wheat(data.Id);
                case ResourceTypeData.Sheep: return new Sheep(data.Id);
                case ResourceTypeData.Clay: return new Clay(data.Id);
                case ResourceTypeData.Ore: return new Ore(data.Id);
                case ResourceTypeData.Dummy: return new DummyResource(data.Id);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public ChitData ToChitData(IChit chit)
        {
            ChitTypeData chitType = ChitTypeData.Chit2;
            if (chit is Chit2) chitType = ChitTypeData.Chit2;
            if (chit is Chit4) chitType = ChitTypeData.Chit4;
            if (chit is Chit5) chitType = ChitTypeData.Chit5;
            if (chit is Chit6) chitType = ChitTypeData.Chit6;
            if (chit is Chit8) chitType = ChitTypeData.Chit8;
            if (chit is Chit9) chitType = ChitTypeData.Chit9;
            if (chit is Chit10) chitType = ChitTypeData.Chit10;
            if (chit is Chit11) chitType = ChitTypeData.Chit11;
            if (chit is Chit12) chitType = ChitTypeData.Chit12;
            if (chit is RandomChit) chitType = ChitTypeData.RandomChit;
            return new ChitData
            {
                Id = chit.Id, ChitType = chitType
            };
        }

        public IChit FromData(ChitData data)
        {
            switch (data.ChitType)
            {
                case ChitTypeData.Chit2: return new Chit2(data.Id);
                case ChitTypeData.Chit3: return new Chit3(data.Id);
                case ChitTypeData.Chit4: return new Chit4(data.Id);
                case ChitTypeData.Chit5: return new Chit5(data.Id);
                case ChitTypeData.Chit6: return new Chit6(data.Id);
                case ChitTypeData.Chit8: return new Chit8(data.Id);
                case ChitTypeData.Chit9: return new Chit9(data.Id);
                case ChitTypeData.Chit10: return new Chit10(data.Id);
                case ChitTypeData.Chit11: return new Chit11(data.Id);
                case ChitTypeData.Chit12: return new Chit12(data.Id);
                case ChitTypeData.RandomChit: return new RandomChit(data.Id);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public PortData ToPortData(IPort port)
        {
            PortTypeData type = PortTypeData.FourToOne;
            if (port is FourToOnePort) type = PortTypeData.FourToOne;
            if (port is ThreeToOnePort) type = PortTypeData.ThreeToOne;
            if (port is TimberPort) type = PortTypeData.Timber;
            if (port is WheatPort) type = PortTypeData.Wheat;
            if (port is SheepPort) type = PortTypeData.Sheep;
            if (port is ClayPort) type = PortTypeData.Clay;
            if (port is OrePort) type = PortTypeData.Ore;
            if (port is RandomPort) type = PortTypeData.RandomPort;
            return new PortData
            {
                Id = port.Id, PortType = type
            };
        }

        public IPort FromData(PortData data)
        {
            var id = data.Id;
            var waterLocation = data.WaterLocation != null ? FromData(data.WaterLocation) : null;
            var landLocation = data.LandLocation != null ? FromData(data.LandLocation) : null;
            switch (data.PortType)
            {
                case PortTypeData.FourToOne: return new FourToOnePort(id, waterLocation, landLocation);
                case PortTypeData.ThreeToOne: return new ThreeToOnePort(id, waterLocation, landLocation);
                case PortTypeData.Timber: return new TimberPort(id, waterLocation, landLocation);
                case PortTypeData.Wheat: return new WheatPort(id, waterLocation, landLocation);
                case PortTypeData.Sheep: return new SheepPort(id, waterLocation, landLocation);
                case PortTypeData.Clay: return new ClayPort(id, waterLocation, landLocation);
                case PortTypeData.Ore: return new OrePort(id, waterLocation, landLocation);
                case PortTypeData.RandomPort: return new RandomPort(id, waterLocation, landLocation);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public HexData ToHexData(IHex hex)
        {
            HexTypeData type = HexTypeData.Forest;
            if (hex is Forest) type = HexTypeData.Forest;
            if (hex is Field) type = HexTypeData.Field;
            if (hex is Pasture) type = HexTypeData.Pasture;
            if (hex is Hill) type = HexTypeData.Hill;
            if (hex is Mountain) type = HexTypeData.Mountain;
            if (hex is Water) type = HexTypeData.Water;
            if (hex is Desert) type = HexTypeData.Desert;
            if (hex is RandomHex) type = HexTypeData.RandomHex;
            var chit = hex.Chit != null ? ToChitData(hex.Chit) : null;
            var port = hex.Port != null ? ToPortData(hex.Port) : null;
            return new HexData
            {
                Id = hex.Id, HexType = type, Location = ToLocationData(hex.Location), Chit = chit, Port = port
            };
        }

        public IHex FromData(HexData data)
        {
            var location = FromData(data.Location);
            IHex hex;
            switch (data.HexType)
            {
                case HexTypeData.Forest: hex = new Forest(data.Id, location); break;
                case HexTypeData.Field: hex = new Field(data.Id, location); break;
                case HexTypeData.Pasture: hex = new Pasture(data.Id, location); break;
                case HexTypeData.Hill: hex = new Hill(data.Id, location); break;
                case HexTypeData.Mountain: hex = new Mountain(data.Id, location); break;
                case HexTypeData.Water: hex = new Water(data.Id, location); break;
                case HexTypeData.Desert: hex = new Desert(data.Id, location); break;
                case HexTypeData.RandomHex: hex = new RandomHex(data.Id, location); break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            hex.Chit = data.Chit != null ? FromData(data.Chit) : null;
            //            var port = data.Port != null ? FromData(data.Port) : null;
            return hex;
        }

        public PieceData ToPieceData(IPiece piece)
        {
            PieceTypeData type = PieceTypeData.City;
            EdgeData edge = null;
            VertexData vertex = null;
            var road = piece as Road;
            if (road != null)
            {
                type = PieceTypeData.Road;
                edge = road.Edge != null ? ToEdgeData(road.Edge) : null;
            }
            var town = piece as Town;
            if (town != null)
            {
                type = PieceTypeData.Town;
                vertex = town.Vertex != null ? ToVertexData(town.Vertex) : null;
            }
            var city = piece as City;
            if (city != null)
            {
                type = PieceTypeData.City;
                vertex = city.Vertex != null ? ToVertexData(city.Vertex) : null;
            }
            if (piece is RoadBuilding.Token) type = PieceTypeData.RoadBuildingToken;
            return new PieceData
            {
                Id = piece.Id,
                PieceType = type,
                PlayerId = piece.Player.Id,
                Edge = edge,
                Vertex = vertex,
            };
        }

        public IPiece FromData(PieceData data, IRepository repo)
        {
            var player = data.PlayerId.HasValue ? repo.Get<IPlayer>(data.PlayerId.Value) : null;
            var edge = data.Edge != null ? FromData(data.Edge) : null;
            var vertex = data.Vertex != null ? FromData(data.Vertex) : null;
            switch (data.PieceType)
            {
                case PieceTypeData.Road: return new Road(player, data.Id, edge);
                case PieceTypeData.Town: return new Town(player, data.Id, vertex);
                case PieceTypeData.City: return new City(player, data.Id, vertex);
                case PieceTypeData.RoadBuildingToken: return new RoadBuilding.Token(player, data.Id);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public DevelopmentCardData ToDevelopmentCardData(IDevelopmentCard developmentCard)
        {
            DevelopmentCardData developmentCardData = null;
            var soldier = developmentCard as Soldier;
            if (soldier != null)
            {
                developmentCardData = new SoldierData
                {
                    DevelopmentCardType = DevelopmentCardTypeData.Soldier
                };
            }
            var victoryPointCard = developmentCard as VictoryPointCard;
            if (victoryPointCard != null)
            {
                developmentCardData = new VictoryPointCardData
                {
                    DevelopmentCardType = DevelopmentCardTypeData.VictoryPoint
                };
            }
            var invention = developmentCard as Invention;
            if (invention != null)
            {
                var pickedResources = invention.PickedResources?
                    .Select(ToResourceData)
                    .ToList();
                developmentCardData = new InventionData
                {
                    DevelopmentCardType = DevelopmentCardTypeData.Invention,
                    PickedResources = pickedResources,

                };
            }
            var monopoly = developmentCard as Monopoly;
            if (monopoly != null)
            {
                developmentCardData = new MonopolyData
                {
                    DevelopmentCardType = DevelopmentCardTypeData.Monopoly,
                    PickedResourceType = monopoly.ResourceType.Value
                };
            }
            var roadBuilding = developmentCard as RoadBuilding;
            if (roadBuilding != null)
            {
                developmentCardData = new RoadBuildingData
                {
                    DevelopmentCardType = DevelopmentCardTypeData.RoadBuilding,
                    Tokens = roadBuilding.Tokens?.Select(rbt => new RoadBuildingTokenData
                    {
                        Id = rbt.Id,
                        PlayerId = rbt.Player.Id
                    })
                    .ToList()
                };
            }
            var dummyDevelopmentCard = developmentCard as DummyDevelopmentCard;
            if (dummyDevelopmentCard != null)
            {
                developmentCardData = new DummyDevelopmentCardData
                {
                    DevelopmentCardType = DevelopmentCardTypeData.Dummy
                };
            }

            developmentCardData.Id = developmentCard.Id;
            developmentCardData.TurnBoughtId = developmentCard.TurnBought?.Id;
            developmentCardData.TurnPlayedId = developmentCard.TurnPlayed?.Id;
            return developmentCardData;
        }

//        public IDevelopmentCard FromData(DevelopmentCardData data, IRepository repo)
//        {
//            IDevelopmentCard developmentCard = null;
//            switch (data.DevelopmentCardType)
//            {
//                case DevelopmentCardTypeData.Soldier: developmentCard = new Soldier(data.Id); break;
//                case DevelopmentCardTypeData.VictoryPoint: developmentCard = new VictoryPointCard(data.Id); break;
//                case DevelopmentCardTypeData.Dummy: developmentCard = new DummyDevelopmentCard(data.Id); break;
//                case DevelopmentCardTypeData.Invention:
//                {
//                    var invention = new Invention(data.Id);
//                        invention.PickedResources = 
//                    break;
//                }
//            }
//            developmentCard.Player = data.PlayerId.HasValue ? repo.Get<IPlayer>(data.PlayerId.Value) : null;
//            developmentCard.TurnBought = data.TurnBoughtId.HasValue ? repo.Get<IPlayTurnsTurn>(data.TurnBoughtId.Value) : null;
//            developmentCard.TurnBought = data.TurnPlayedId.HasValue ? repo.Get<IPlayTurnsTurn>(data.TurnPlayedId.Value) : null;
//        }
    }
}
