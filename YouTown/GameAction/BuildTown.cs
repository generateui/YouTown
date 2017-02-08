using System.Linq;
using YouTown.Validation;

namespace YouTown.GameAction
{
    public class BuildTown : IGameAction
    {
        public static ActionType BuildTownType = new ActionType("BuildTown");
        public BuildTown(int id, IPlayer player, Point point)
        {
            Id = id;
            Player = player;
            Point = point;
        }

        public int Id { get; }
        public ActionType ActionType => BuildTownType;
        public IPlayer Player { get; }
        public ITurnPhase TurnPhase { get; private set; }
        public IGamePhase GamePhase { get; private set; }
        public ITurn Turn { get; private set; }
        public bool IsAllowedInOpponentTurn => false;
        public Point Point { get; }

        public bool IsAllowedInTurnPhase(ITurnPhase tp) => tp.IsBuilding;
        public bool IsAllowedInGamePhase(IGamePhase gp) => gp.IsTurns || gp.IsInitialPlacement;

        public IValidationResult Validate(IGame game) =>
            new ValidateAll()
                .WithObject<NotNull>(Point)
                .With<HasTownInStock, IPlayer>(Player)
//                .With<TownAllowedWhenInitialPlacement>(game.GamePhase) TODO: implement
                .With<IsOnTurn, IPlayer>(Player)
                .With<CanBuildTownAt, Point, IBoard>(Point, game.Board)
                .With<HasRoadToPoint, Point, IPlayer>(Point, Player)
                .Validate();

        public void PerformAtServer(IServerGame serverGame)
        {
        }

        public void Perform(IGame game)
        {
            var town = Player.Stock[Town.TownType].Last() as Town;
            game.GamePhase.BuildTown(game, Player, town);
            town.Point = Point;
            town.AddToPlayer(Player);
            town.AddToBoard(game.Board);
            var matchingHexWithPort = game.Board.HexesByLocation.Values
                .Where(h => h.Port != null)
                .FirstOrDefault(h => h.Port.Edge.Points.Contains(Point));
            if (matchingHexWithPort != null)
            {
                Player.Ports.Add(matchingHexWithPort.Port);
            }

            TurnPhase = game.PlayTurns.TurnPhase;
            GamePhase = game.GamePhase;
            Turn = game.PlayTurns.Turn;
        }
    }
}
