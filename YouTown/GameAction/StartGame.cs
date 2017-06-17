using System;
using System.Collections.Generic;
using YouTown.Validation;

namespace YouTown.GameAction
{
    public sealed class StartGame : GameActionBase
    {
        public static ActionType StartGameType = new ActionType("StartGame");

        public StartGame(int id, IPlayer player) : base(id, player) { }
        public StartGame(StartGameData data, IRepository repo) : base(data, repo)
        {
            Game = data.Game != null ? new Game(data.Game) : null;
        }

        public override ActionType ActionType => StartGameType;
        public IGame Game { get; set; }

        public override bool IsAllowedInTurnPhase(ITurnPhase turnPhase) => true;
        public override bool IsAllowedInGamePhase(IGamePhase gamePhase) => gamePhase.IsDetermineFirstPlayer;

        public override GameActionData ToData() =>
            ToData(new StartGameData
            {
                GameActionType = GameActionTypeData.StartGame,
                Game = Game?.ToData()
            });

        public override IValidationResult Validate(IGame game)
        {
            throw new NotImplementedException();
        }

        public override void PerformAtServer(IServerGame serverGame)
        {
            // server should 
            // - instantiate the game 
            // - add the users
//            Game.Status = new Playing()
            var players = new List<IPlayer>();
            var identifier = serverGame.Identifier;
            foreach (IUser user in serverGame.Users)
            {
                var player = new Player(identifier.NewId(), user.Color, user);
                players.Add(player);
                serverGame.PlayersByUser[user] = player;
            }
            var options = serverGame.SetupOptions;
            var playBoard = options.BoardDesign.Setup(options.Ports, options.Chits, options.Hexes, serverGame.Random);

            foreach (var player in players)
            {
                var roads = new List<IPiece>();
                for (int i = 0; i < options.RoadCount; i++)
                {
                    roads.Add(new Road(player, identifier.NewId()));
                }
                player.Stock[Road.RoadType] = roads;

                var towns = new List<IPiece>();
                for (int i = 0; i < options.TownCount; i++)
                {
                    towns.Add(new Town(player, identifier.NewId()));
                }
                player.Stock[Town.TownType] = towns;

                var cities = new List<IPiece>();
                for (int i = 0; i < options.CityCount; i++)
                {
                    cities.Add(new City(player, identifier.NewId()));
                }
                player.Stock[City.CityType] = cities;

                serverGame.DevelopmentCardsByPlayer[player] = new List<IDevelopmentCard>();
            }

            var resources = new List<IResource>();
            foreach (KeyValuePair<ResourceType, int> pair in options.ResourceCountByType)
            {
                var resourceType = pair.Key;
                var amount = pair.Value;
                for (int i = 0; i < amount; i++)
                {
                    var id = identifier.NewId();
                    var resource = resourceType.Create(id);
                    resources.Add(resource);
                }
            }

            var developmentCards = new List<IDevelopmentCard>();
            foreach (KeyValuePair<DevelopmentCardType, int> pair in options.DevelopmentCardCountByType)
            {
                var developmentCardType = pair.Key;
                var amount = pair.Value;
                for (int i = 0; i < amount; i++)
                {
                    var id = identifier.NewId();
                    var developmentCard = developmentCardType.Create(id);
                    developmentCards.Add(developmentCard);
                }
            }

            var bankResources = new ResourceList(resources);
            var bank = new Bank(bankResources, developmentCards);

            var playerList = new PlayerList(players);
            Game = new Game(playBoard, bank, playerList, new PlayOptions());

            Game.LargestArmy = new LargestArmy(identifier.NewId());

//            Game.LongestRoad = new LongestRoad(identifier.NewId());
        }

        public override void Perform(IGame game)
        {
            game.GamePhase.Start(game);

            base.Perform(game);
        }
    }
}
