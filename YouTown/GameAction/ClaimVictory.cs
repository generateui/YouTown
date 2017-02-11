using YouTown.Validation;

namespace YouTown.GameAction
{
    public class ClaimVictory : GameActionBase
    {
        public static ActionType ClaimVictoryType = new ActionType("ClaimVictory");

        public ClaimVictory(int id, IPlayer player) : base(id, player) { }
        public ClaimVictory(ClaimVictoryData data, IRepository repo) : base(data, repo) { }

        public override ActionType ActionType => ClaimVictoryType;

        public override bool IsAllowedInTurnPhase(ITurnPhase tp) => true;
        public override bool IsAllowedInGamePhase(IGamePhase gp) => gp.IsTurns;

        public override GameActionData ToData() =>
            base.ToData(new ClaimVictoryData
            {
                GameActionType = GameActionTypeData.ClaimVictory
            });

        public override IValidationResult Validate(IGame game) =>
            new ValidateAll()
                .With<HasEnoughVictoryPoints, IPlayer, IGame>(Player, game)
                .Validate();

        public override void Perform(IGame game)
        {
            // TODO: set status here
            game.MoveToNextPhase();

            base.Perform(game);
        }
    }
}
