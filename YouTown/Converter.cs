using System;
using System.Collections.Generic;
using System.Linq;
using Bond;
using YouTown.GameAction;

namespace YouTown
{
    public static class ConversionExtensions
    {
        public static ISet<T> ToSet<T>(this IEnumerable<T> enumerable)
        {
            return new HashSet<T>(enumerable);
        }
        public static IEnumerable<T> FromRepo<T>(this IEnumerable<int> ids, IRepository repo)
            where T : IGameItem
        {
            return ids.Select(repo.Get<T>);
        }

        public static List<int> ToIdList(this IEnumerable<IGameItem> items) =>
            items.Select(i => i.Id).ToList();

        public static IResourceList FromData(this IList<ResourceData> data) =>
            new ResourceList(data.Select(rd => rd.FromData()));

        public static List<ResourceData> ToData(this IResourceList resourceList) =>
            resourceList.Select(r => r.ToData()).ToList();

        public static IResource FromData(this ResourceData data)
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

        public static ResourceData ToData(this IResource resource)
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

        public static ChitData ToData(this IChit chit)
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
                Id = chit.Id,
                ChitType = chitType
            };
        }

        public static IChit FromData(this ChitData data)
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

        public static PortData ToData(this IPort port)
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
                Id = port.Id,
                PortType = type
            };
        }

        public static IPort FromData(this PortData data)
        {
            var id = data.Id;
            var waterLocation = data.WaterLocation != null ? new Location(data.WaterLocation) : null;
            var landLocation = data.LandLocation != null ? new Location(data.LandLocation) : null;
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

        public static HexData ToData(this IHex hex)
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
            var chit = hex.Chit?.ToData();
            var port = hex.Port?.ToData();
            return new HexData
            {
                Id = hex.Id,
                HexType = type,
                Location = hex.Location.ToData(),
                Chit = chit,
                Port = port
            };
        }

        public static IHex FromData(this HexData data)
        {
            var location = new Location(data.Location);
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
            hex.Chit = data.Chit?.FromData();
            hex.Port = data.Port?.FromData();
            return hex;
        }

        public static IBoardDesign FromData(this BoardDesignData data)
        {
            var hexes = data.Hexes.Select(h => h.FromData()).ToDictionary(h => h.Location, h => h);
            return new BoardDesign(data.Name, hexes);
        }

        public static BoardDesignData ToData(this IBoardDesign boardDesign) =>
            new BoardDesignData
            {
                Name = boardDesign.Name,
                Hexes = boardDesign.HexesByLocation.Values.Select(h => h.ToData()).ToList(),
            };

        public static TradeOfferResponseData ToData(this ITradeOfferResponse tradeOfferResponse)
        {
            if (tradeOfferResponse is Accept)
            {
                return new TradeOfferResponseData
                {
                    TradeOfferResponseType = TradeOfferResponseTypeData.Accept,
                    PlayerId = tradeOfferResponse.Player.Id
                };
            }
            if (tradeOfferResponse is Reject)
            {
                return new TradeOfferResponseData
                {
                    TradeOfferResponseType = TradeOfferResponseTypeData.Reject,
                    PlayerId = tradeOfferResponse.Player.Id
                };
            }
            var offer = tradeOfferResponse as CounterOffer;
            if (offer != null)
            {
                return new TradeOfferResponseData
                {
                    TradeOfferResponseType = TradeOfferResponseTypeData.Counter,
                    PlayerId = tradeOfferResponse.Player.Id,
                    CounterOffered = tradeOfferResponse.CounterOffered.ToData(),
                    CounterRequested = tradeOfferResponse.CounterRequested.ToData(),
                };
            }
            throw new ArgumentException($"unknown {nameof(ITradeOfferResponse)}");
        }

        public static ITradeOfferResponse FromData(this TradeOfferResponseData data, IRepository repo)
        {
            var player = repo.Get<IPlayer>(data.PlayerId);
            switch (data.TradeOfferResponseType)
            {
                case TradeOfferResponseTypeData.Accept: return new Accept(data.Id, player);
                case TradeOfferResponseTypeData.Reject: return new Reject(data.Id, player);
                case TradeOfferResponseTypeData.Counter:
                    var counterOffered = data.CounterOffered?.FromData();
                    var counterRequested = data.CounterRequested?.FromData();
                    return new CounterOffer(data.Id, player, counterOffered, counterRequested);
                case TradeOfferResponseTypeData.Unknown:
                    throw new ArgumentOutOfRangeException();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static PieceData ToData(this IPiece piece)
        {
            var road = piece as Road;
            if (road != null)
            {
                return new RoadData
                {
                    Id = piece.Id,
                    PieceType = PieceTypeData.Road,
                    PlayerId = piece.Player?.Id,
                    Edge = road.Edge?.ToData(),
                };
            }
            var town = piece as Town;
            if (town != null)
            {
                return new TownData
                {
                    Id = piece.Id,
                    PieceType = PieceTypeData.Town,
                    PlayerId = piece.Player?.Id,
                    Vertex = town.Vertex?.ToData(),
                };
            }
            var city = piece as City;
            if (city != null)
            {
                return new CityData
                {
                    Id = piece.Id,
                    PieceType = PieceTypeData.City,
                    PlayerId = piece.Player?.Id,
                    Vertex = city.Vertex?.ToData(),
                };
            }
            if (piece is RoadBuilding.Token)
            {
                return new RoadBuildingTokenData
                {
                    Id = piece.Id,
                    PieceType = PieceTypeData.RoadBuildingToken,
                    PlayerId = piece.Player?.Id
                };
            }
            throw new ArgumentOutOfRangeException();
        }

        public static IPiece FromData(this IBonded<PieceData> bondedData, IRepository repo)
        {
            var data = bondedData.Deserialize();
            var player = data.PlayerId.HasValue ? repo.Get<IPlayer>(data.PlayerId.Value) : null;
            switch (data.PieceType)
            {
                case PieceTypeData.Road:
                    var roadData = bondedData.Deserialize<RoadData>();
                    var edge = roadData.Edge != null ? new Edge(roadData.Edge) : null;
                    return new Road(player, data.Id, edge);
                case PieceTypeData.Town:
                    var townData = bondedData.Deserialize<TownData>();
                    var vertex1 = townData.Vertex != null ? new Vertex(townData.Vertex) : null;
                    return new Town(player, data.Id, vertex1);
                case PieceTypeData.City:
                    var cityData = bondedData.Deserialize<CityData>();
                    var vertex2 = cityData.Vertex != null ? new Vertex(cityData.Vertex) : null;
                    return new City(player, data.Id, vertex2);
                case PieceTypeData.RoadBuildingToken: return new RoadBuilding.Token(player, data.Id);
                case PieceTypeData.Unknown:
                    throw new ArgumentOutOfRangeException();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static DevelopmentCardData ToData(this IDevelopmentCard developmentCard)
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
                var pickedResources = invention.PickedResources?.ToData();
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
            if (developmentCardData == null)
            {
                throw new ArgumentOutOfRangeException();
            }
            developmentCardData.Id = developmentCard.Id;
            developmentCardData.TurnBoughtId = developmentCard.TurnBought?.Id;
            developmentCardData.TurnPlayedId = developmentCard.TurnPlayed?.Id;
            return developmentCardData;
        }

        public static IDevelopmentCard FromData(this IBonded<DevelopmentCardData> bonded, IRepository repo)
        {
            IDevelopmentCard developmentCard;
            var data = bonded.Deserialize();
            var player = data.PlayerId.HasValue ? repo.Get<IPlayer>(data.PlayerId.Value) : null;
            switch (data.DevelopmentCardType)
            {
                case DevelopmentCardTypeData.Soldier: developmentCard = new Soldier(data.Id); break;
                case DevelopmentCardTypeData.VictoryPoint: developmentCard = new VictoryPointCard(data.Id); break;
                case DevelopmentCardTypeData.Dummy: developmentCard = new DummyDevelopmentCard(data.Id); break;
                case DevelopmentCardTypeData.Invention:
                    var inventionData = bonded.Deserialize<InventionData>();
                    developmentCard = new Invention(data.Id)
                    {
                        PickedResources = inventionData.PickedResources.FromData()
                    };
                    break;
                case DevelopmentCardTypeData.Monopoly:
                    var monopolyData = bonded.Deserialize<MonopolyData>();
                    developmentCard = new Monopoly(data.Id)
                    {
                        ResourceType = ResourceType.Parse(monopolyData.PickedResourceType)
                    };
                    break;
                case DevelopmentCardTypeData.RoadBuilding:
                    var roadBuildingData = bonded.Deserialize<RoadBuildingData>();
                    developmentCard = new RoadBuilding(data.Id)
                    {
                        Tokens = roadBuildingData.Tokens.Select(td => new RoadBuilding.Token(player, td.Id))
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            developmentCard.Player = player;
            developmentCard.TurnBought = data.TurnBoughtId.HasValue ? repo.Get<IPlayTurnsTurn>(data.TurnBoughtId.Value) : null;
            developmentCard.TurnBought = data.TurnPlayedId.HasValue ? repo.Get<IPlayTurnsTurn>(data.TurnPlayedId.Value) : null;
            return developmentCard;
        }

        public static PlayTurnsTurnData ToData(this IPlayTurnsTurn playTurnsTurn) =>
            new PlayTurnsTurnData
            {
                Id = playTurnsTurn.Id,
                Number = playTurnsTurn.Number,
                HasPlayedDevelopmentCard = playTurnsTurn.HasPlayedDevelopmentCard,
                TradeOffers = playTurnsTurn.TradeOffers?.Select(to => to.ToData()).ToList()
            };

        public static PlayTurnsTurn FromData(this PlayTurnsTurnData data, IRepository repo)
        {
            var player = repo.Get<IPlayer>(data.PlayerId);
            var tradeOffers = data.TradeOffers?.Select(to => new TradeOffer(to, repo)).ToList();
            return new PlayTurnsTurn(data.Id, data.Number, player, tradeOffers)
            {
                HasPlayedDevelopmentCard = data.HasPlayedDevelopmentCard,
            };
        }

        public static ISetupOptions FromData(this SetupOptionsData data) => 
            new SetupOptions
            {
                Name = data.Name,
                BoardDesign = data.BoardDesign.FromData(),
                RoadCount = data.RoadCount,
                TownCount = data.TownCount,
                CityCount = data.CityCount,
                Ports = data.Ports.Select(p => p.FromData()).ToList(),
                Hexes = data.Hexes.Select(h => h.FromData()).ToList(),
                Chits = data.Chits.Select(c => c.FromData()).ToList(),
                ResourceCountByType = data.ResourceCountByType
                        .ToDictionary(x => ResourceType.Parse(x.Key), x => x.Value),
                DevelopmentCardCountByType = data.DevelopmnetCardCountByType
                        .ToDictionary(x => DevelopmentCardType.Parse(x.Key), x => x.Value)
            };

        public static SetupOptionsData ToData(this ISetupOptions setupOptions) =>
            new SetupOptionsData
            {
                Name = setupOptions.Name,
                BoardDesign = setupOptions.BoardDesign.ToData(),
                RoadCount = setupOptions.RoadCount,
                TownCount = setupOptions.TownCount,
                CityCount = setupOptions.CityCount,
                Ports = setupOptions.Ports.Select(p => p.ToData()).ToList(),
                Hexes = setupOptions.Hexes.Select(h => h.ToData()).ToList(),
                Chits = setupOptions.Chits.Select(c => c.ToData()).ToList(),
                ResourceCountByType = setupOptions.ResourceCountByType
                    .ToDictionary(x => x.Key.Value, x => x.Value),
                DevelopmnetCardCountByType = setupOptions.DevelopmentCardCountByType
                    .ToDictionary(x => x.Key.Value, x => x.Value)
            };

        public static SetupData ToData(this SetupGamePhase gamePhase) =>
            new SetupData
            {
                GamePhaseType = GamePhaseTypeData.Setup
            };

        public static SetupGamePhase FromData(this SetupData data) => new SetupGamePhase(data.Id);

        public static DetermineFirstPlayerData ToData(this DetermineFirstPlayer gamePhase) =>
            new DetermineFirstPlayerData
            {
                GamePhaseType = GamePhaseTypeData.Setup
            };

        public static DetermineFirstPlayer FromData(this DetermineFirstPlayerData data) => new DetermineFirstPlayer(data.Id);

        public static EndOfGameData ToData(this EndOfGame gamePhase) =>
            new EndOfGameData
            {
                GamePhaseType = GamePhaseTypeData.End
            };

        public static EndOfGame FromData(this EndOfGameData data) => new EndOfGame(data.Id);

        public static IBank FromData(this BankData data, IRepository repo)
        {
            var resources = data.Resources.FromData();
            var developmentCards = data.DevelopmentCards.Select(dc => dc.FromData(repo)).ToList();
            return new Bank(resources, developmentCards);
        }

        public static BankData ToData(this IBank bank) =>
            new BankData
            {
                Resources = bank.Resources.ToData(),
                DevelopmentCards = bank.DevelopmentCards
                    .Select(dc => new Bonded<DevelopmentCardData>(dc.ToData()))
                    .Cast<IBonded<DevelopmentCardData>>()
                    .ToList()
            };

        public static UserData ToData(this IUser user) =>
            new UserData
            {
                Id = user.Id,
                Name = user.Name,
                Color = user.Color.Value
            };

        public static IPlayer FromData(this PlayerData data, IRepository repo)
        {
            var user = repo.Get<IUser>(data.UserId);
            var color = PlayerColor.Parse(data.Color);
            var player = new Player(data.Id, color, user);
            repo.Add(player);
            var pieces = data.Pieces.Select(pd => pd.FromData(repo)).ToList();
            repo.AddAll(pieces);
            player.Pieces.AddEnumerable(pieces);
            var stockPieces = data.StockPieces.Select(sp => sp.FromData(repo));
            var stock = new Dictionary<PieceType, IList<IPiece>>();
            foreach (var stockPiece in stockPieces)
            {
                var pieceType = stockPiece.PieceType;
                if (!stock.ContainsKey(pieceType))
                {
                    stock[pieceType] = new List<IPiece> { stockPiece };
                }
                else
                {
                    stock[pieceType].Add(stockPiece);
                }
            }

            player.Stock.AddDictionary(stock);
            player.DevelopmentCards.AddEnumerable(data.DevelopmentCards.Select(dc => dc.FromData(repo)));
            player.PlayedDevelopmentCards.AddEnumerable(data.PlayedDevelopmentCards.Select(dc => dc.FromData(repo)));
            player.Hand.AddEnumerable(data.Hand.Select(r => r.FromData()));
            player.Producers.AddEnumerable(data.ProducerIds.Select(repo.Get<IProducer>));
            var edgePieces = data.EdgePieceIds.Select(repo.Get<IEdgePiece>);
            var edgePiecesLists = new Dictionary<Edge, IList<IEdgePiece>>();
            foreach (var edgePiece in edgePieces)
            {
                var edge = edgePiece.Edge;
                if (!edgePiecesLists.ContainsKey(edge))
                {
                    edgePiecesLists[edge] = new List<IEdgePiece> { edgePiece };
                }
                else
                {
                    edgePiecesLists[edge].Add(edgePiece);
                }
            }
            player.EdgePieces.AddDictionary(edgePiecesLists);

            var vertexPieces = data.VertexPieceIds.Select(repo.Get<IVertexPiece>);
            var vertexPieceLists = new Dictionary<Vertex, IList<IVertexPiece>>();
            foreach (var vertexPiece in vertexPieces)
            {
                var vertex = vertexPiece.Vertex;
                if (!vertexPieceLists.ContainsKey(vertex))
                {
                    vertexPieceLists[vertex] = new List<IVertexPiece> { vertexPiece };
                }
                else
                {
                    vertexPieceLists[vertex].Add(vertexPiece);
                }
            }
            player.Ports.AddEnumerable(data.PortIds.Select(repo.Get<IPort>));
            player.VictoryPoints.AddEnumerable(data.VictoryPointIds.Select(repo.Get<IVictoryPoint>));
            player.Soldiers.AddEnumerable(data.SoldierIds.Select(repo.Get<Soldier>));
            return player;
        }

        public static PlayerData ToData(this IPlayer player)
        {
            var stockPieces = player.Stock.Values
                .SelectMany(v => v)
                .Select(sp => new Bonded<PieceData>(sp.ToData()))
                .Cast<IBonded<PieceData>>()
                .ToList();
            var pieces = player.Pieces
                .Select(p => new Bonded<PieceData>(p.ToData()))
                .Cast<IBonded<PieceData>>()
                .ToList();
            var developmentCards = player.DevelopmentCards
                .Select(dc => new Bonded<DevelopmentCardData>(dc.ToData()))
                .Cast<IBonded<DevelopmentCardData>>()
                .ToList();
            var playedDevelopmentCards = player.PlayedDevelopmentCards
                .Select(dc => new Bonded<DevelopmentCardData>(dc.ToData()))
                .Cast<IBonded<DevelopmentCardData>>()
                .ToList();

            return new PlayerData
            {
                Id = player.Id,
                UserId = player.User.Id,
                Color = player.Color.Value,
                IsOnTurn = player.IsOnTurn,
                StockPieces = stockPieces,
                Pieces = pieces,
                DevelopmentCards = developmentCards,
                PlayedDevelopmentCards = playedDevelopmentCards,
                Hand = player.Hand.ToData(),
                ProducerIds = player.Producers.Select(p => p.Id).ToList(),
                EdgePieceIds = player.EdgePieces.Values
                    .SelectMany(eps => eps)
                    .Select(ep => ep.Id)
                    .ToList(),
                VertexPieceIds = player.VertexPieces.Values
                    .SelectMany(vps => vps)
                    .Select(vp => vp.Id)
                    .ToList(),
                PortIds = player.Ports.Select(p => p.Id).ToList(),
                VictoryPointIds = player.VictoryPoints.Select(vp => vp.Id).ToList(),
                SoldierIds = player.Soldiers.Select(s => s.Id).ToList()
            };
        }

        public static IGameAction FromData(this IBonded<GameActionData> bonded, IRepository repo)
        {
            var data = bonded.Deserialize();
            switch (data.GameActionType)
            {
                case GameActionTypeData.AcceptTradeOffer:
                    return new AcceptTradeOffer(bonded.Deserialize<AcceptTradeOfferData>(), repo);
                case GameActionTypeData.BuildCity:
                    return new BuildCity(bonded.Deserialize<BuildCityData>(), repo);
                case GameActionTypeData.BuildRoad:
                    return new BuildRoad(bonded.Deserialize<BuildRoadData>(), repo);
                case GameActionTypeData.BuildTown:
                    return new BuildTown(bonded.Deserialize<BuildTownData>(), repo);
                case GameActionTypeData.BuyDevelopmentCard:
                    return new BuyDevelopmentCard(bonded.Deserialize<BuyDevelopmentCardData>(), repo);
                case GameActionTypeData.ClaimVictory:
                    return new ClaimVictory(bonded.Deserialize<ClaimVictoryData>(), repo);
                case GameActionTypeData.CounterTradeOffer:
                    return new CounterOfferTrade(bonded.Deserialize<CounterOfferTradeData>(), repo);
                case GameActionTypeData.EndTurn:
                    return new EndTurn(bonded.Deserialize<EndTurnData>(), repo);
                case GameActionTypeData.LooseCards:
                    return new LooseCards(bonded.Deserialize<LooseCardsData>(), repo);
                case GameActionTypeData.MoveRobber:
                    return new MoveRobber(bonded.Deserialize<MoveRobberData>(), repo);
                case GameActionTypeData.OfferTrade:
                    return new OfferTrade(bonded.Deserialize<OfferTradeData>(), repo);
                case GameActionTypeData.PlayDevelopmentCard:
                    return new PlayDevelopmentCard(bonded.Deserialize<PlayDevelopmentCardData>(), repo);
                case GameActionTypeData.RejectTradeOffer:
                    return new RejectTradeOffer(bonded.Deserialize<RejectTradeOfferData>(), repo);
                case GameActionTypeData.RobPlayer:
                    return new RobPlayer(bonded.Deserialize<RobPlayerData>(), repo);
                case GameActionTypeData.RollDice:
                    return new RollDice(bonded.Deserialize<RollDiceData>(), repo);
                case GameActionTypeData.SaySomething:
                    return new SaySomething(bonded.Deserialize<SaySomethingData>(), repo);
                case GameActionTypeData.StartGame:
                    return new StartGame(bonded.Deserialize<StartGameData>(), repo);
                case GameActionTypeData.TradeWithBank:
                    return new TradeWithBank(bonded.Deserialize<TradeWithBankData>(), repo);
                case GameActionTypeData.TradeWithPlayer:
                    return new TradeWithPlayer(bonded.Deserialize<TradeWithPlayerData>(), repo);
                case GameActionTypeData.Unknown:
                    throw new ArgumentOutOfRangeException();
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public static IActionQueue FromData(this List<QueuedItemGroupData> data, IRepository repo)
        {
            var queue = new ActionQueue();
            foreach (QueuedItemGroupData groupData in data)
            {
                switch (groupData.QueuedItemGroupType)
                {
                    case QueuedItemGroupTypeData.Single:
                        var gameAction = groupData.Actions.FirstOrDefault().FromData(repo);
                        queue.EnqueueSingle(gameAction);
                        break;
                    case QueuedItemGroupTypeData.Unordered:
                        var unorderedActions = groupData.Actions.Select(i => i.FromData(repo));
                        queue.EnqueueUnordered(unorderedActions);
                        break;
                    case QueuedItemGroupTypeData.Ordered:
                        var orderedActions = groupData.Actions.Select(i => i.FromData(repo)).ToList();
                        queue.EnqueueOrdered(orderedActions);
                        break;
                    case QueuedItemGroupTypeData.Unknown:
                        throw new ArgumentOutOfRangeException();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return queue;
        }

        public static IPlayOptions FromData(this PlayOptionsData data) =>
            new PlayOptions
            {
                VictoryPointsToWin = data.VictoryPointsToWin
            };

        public static PlayOptionsData ToData(this IPlayOptions playOptions) =>
            new PlayOptionsData
            {
                VictoryPointsToWin = playOptions.VictoryPointsToWin
            };


        public static BoardForPlayData ToData(this IBoardForPlay boardForPlay) =>
            new BoardForPlayData
            {
                Hexes = boardForPlay.HexesByLocation.Values.Select(h => h.ToData()).ToList(),
                Robber = boardForPlay.Robber?.ToData(),
                PieceIds = boardForPlay.Pieces.ToIdList(),
                ProducerIds = boardForPlay.Producers.ToIdList(),
                RoadIds = boardForPlay.RoadsByEdge.Values.ToIdList(),
                TownIds = boardForPlay.TownsByVertex.Values.ToIdList(),
                CityIds = boardForPlay.CitiesByVertex.Values.ToIdList(),
                VertexPieceIds = boardForPlay.PiecesByVertex.Values.ToIdList(),
                EdgePieceIds = boardForPlay.PiecesByEdge.Values.ToIdList(),
            };


        public static GameData ToData(this IGame game) =>
            new GameData
            {
                Users = game.Users.Select(u => u.ToData()).ToList(),
                Players = game.Players.Select(p => p.ToData()).ToList(),
                PlayOptions = game.PlayOptions.ToData(),
                SetupOptions = game.SetupOptions.ToData(),
                Chats = game.Chats.Select(c => c.ToData()).ToList(),
                Queue = game.Queue.ToData(),
                Actions = game.Actions
                    .Select(a => new Bonded<GameActionData>(a.ToData()))
                    .Cast<IBonded<GameActionData>>()
                    .ToList(),
                Board = game.Board.ToData(),
                Bank = game.Bank.ToData(),
                LargestArmy = game.LargestArmy.ToData(),
                GamePhaseId = game.GamePhase.Id,
                DetermineFirstPlayer = game.DetermineFirstPlayer.ToData(),
                Setup = game.SetupGamePhase.ToData(),
                PlaceInitialPieces = game.PlaceInitialPieces.ToData(),
                PlayTurns = game.PlayTurns.ToData(),
                EndOfGame = game.EndOfGame.ToData(),
            };
    }
}
