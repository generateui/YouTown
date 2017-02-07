using System.Linq;
using YouTown.Validation;

namespace YouTown.GameAction
{
    public class BuildRoad : IGameAction
    {
        public BuildRoad(int id, IPlayer player, Edge edge)
        {
            Id = id;
            Player = player;
            Edge = edge;
        }

        public int Id { get; }
        public IPlayer Player { get; }
        public ITurnPhase TurnPhase { get; private set; }
        public IGamePhase GamePhase { get; private set; }
        public ITurn Turn { get; private set; }
        public Edge Edge { get; }
        public bool IsAllowedInOpponentTurn => false;

        public bool IsAllowedInTurnPhase(ITurnPhase tp) => tp.IsBuilding;
        public bool IsAllowedInGamePhase(IGamePhase gp) => gp.IsTurns || gp.IsInitialPlacement;

        public IValidationResult Validate(IGame game) =>
            new ValidateAll()
                .WithObject<NotNull>(Edge)
                .With<HasRoadInStock, IPlayer>(Player)
                //                .With<RoadAllowedWhenInitialPlacement>(game.GamePhase) TODO: implement
                .With<IsOnTurn, IPlayer>(Player)
                .With<CanBuildRoadAt, Edge, IBoard>(Edge, game.Board)
                .With<HasRoadOrTownToEdge, Edge, IPlayer>(Edge, Player)
                .Validate();

        public void PerformAtServer(IServerGame serverGame)
        {
        }

        public void Perform(IGame game)
        {
            var road = Player.Stock[Road.RoadType].Last() as Road;
            road.Edge = Edge;
            road.AddToPlayer(Player);
            road.AddToBoard(game.Board);
            game.GamePhase.BuildRoad(game, Player, road);
            //            game.MoveLongestRoadIfNeeded(); TODO: implement

            TurnPhase = game.PlayTurns.TurnPhase;
            GamePhase = game.GamePhase;
            Turn = game.PlayTurns.Turn;
        }
    }
}
