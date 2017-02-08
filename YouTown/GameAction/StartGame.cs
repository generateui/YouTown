using System;
using System.Collections.Generic;
using YouTown.Validation;

namespace YouTown.GameAction
{
    public sealed class StartGame : IGameAction
    {
        public static ActionType StartGameType = new ActionType("StartGame");

        public StartGame(int id, IPlayer player)
        {
            Id = id;
            Player = player;
        }

        public ActionType ActionType => StartGameType;
        public int Id { get; }
        public IPlayer Player { get; }
        public ITurnPhase TurnPhase { get; private set; }
        public IGamePhase GamePhase { get; private set; }
        public ITurn Turn { get; private set; }
        public IGame Game { get; set; }

        public bool IsAllowedInOpponentTurn => false;

        public bool IsAllowedInTurnPhase(ITurnPhase turnPhase) => true;
        public bool IsAllowedInGamePhase(IGamePhase gamePhase) => gamePhase.IsDetermineFirstPlayer;

        public IValidationResult Validate(IGame game)
        {
            throw new NotImplementedException();
        }

        public void PerformAtServer(IServerGame serverGame)
        {
            // server should 
            // - instantiate the game 
            // - add the users
//            Game.Status = new Playing()
            var players = new List<IPlayer>();
            foreach (IUser user in serverGame.Users)
            {
                var player = new Player(user);
                players.Add(player);
                serverGame.PlayersByUser[user] = player;
            }
            var options = serverGame.SetupOptions;
            var identifier = serverGame.Identifier;
            foreach (IPlayer player in players)
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
            var playBoard = options.Board.Setup(options.Ports, options.Chits, options.Hexes, serverGame.Random);

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
            Game = new Game(playBoard, bank, playerList);
            Game.LargestArmy = new LargestArmy(identifier.NewId());
//            Game.LongestRoad = new LongestRoad(identifier.NewId());
        }

        public void Perform(IGame game)
        {
            game.GamePhase.Start(game);
        }
    }
}
