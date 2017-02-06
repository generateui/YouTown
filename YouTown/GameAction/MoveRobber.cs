using YouTown.Validator;

namespace YouTown.GameAction
{
    public class MoveRobber : IGameAction
    {
        public MoveRobber(int id, IPlayer player)
        {
            Id = id;
            Player = player;
        }

        public int Id { get; }
        public IPlayer Player { get; }
        public ITurnPhase TurnPhase { get; private set; }
        public IGamePhase GamePhase { get; private set; }
        public ITurn Turn { get; private set; }
        public Location Location { get; set; }
        public bool IsAllowedInOpponentTurn => false;

        public bool IsAllowedInTurnPhase(ITurnPhase tp) => tp.IsBeforeDiceRoll || tp.IsDiceRoll || tp.IsBuilding;
        public bool IsAllowedInGamePhase(IGamePhase gp) => gp.IsTurns;

        public IValidationResult Validate(IGame game) =>
            new ValidateAll()
                .WithObject<NotNull>(Location)
                .With<HasHexAt, IBoard, Location>(game.Board, Location)
                .With<CanPlaceRobberAt, IBoard, Location>(game.Board, Location)
                .Validate();

        public void PerformAtServer(IServerGame serverGame)
        {
        }

        public void Perform(IGame game)
        {
            game.Board.Robber.Location = Location;

            TurnPhase = game.PlayTurns.TurnPhase;
            GamePhase = game.GamePhase;
            Turn = game.PlayTurns.Turn;
        }
    }
}
