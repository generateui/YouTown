using YouTown.Validation;

namespace YouTown.GameAction
{
    public class ClaimVictory : IGameAction
    {
        public static ActionType ClaimVictoryType = new ActionType("ClaimVictory");
        public ClaimVictory(int id, IPlayer player)
        {
            Id = id;
            Player = player;
        }

        public int Id { get; }
        public ActionType ActionType => ClaimVictoryType;
        public IPlayer Player { get; }
        public ITurnPhase TurnPhase { get; private set; }
        public IGamePhase GamePhase { get; private set; }
        public ITurn Turn { get; private set; }
        public bool IsAllowedInOpponentTurn => false;

        public bool IsAllowedInTurnPhase(ITurnPhase tp) => true;
        public bool IsAllowedInGamePhase(IGamePhase gp) => gp.IsTurns;

        public IValidationResult Validate(IGame game) =>
            new ValidateAll()
                .With<HasEnoughVictoryPoints, IPlayer, IGame>(Player, game)
                .Validate();

        public void PerformAtServer(IServerGame serverGame)
        {
        }

        public void Perform(IGame game)
        {
            // TODO: set status here
            game.MoveToNextPhase();

            TurnPhase = game.PlayTurns.TurnPhase;
            GamePhase = game.GamePhase;
            Turn = game.PlayTurns.Turn;
        }
    }
}
