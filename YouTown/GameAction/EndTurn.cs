using System.Linq;
using YouTown.Validation;

namespace YouTown.GameAction
{
    public class EndTurn : GameActionBase
    {
        public static ActionType EndTurnType = new ActionType("EndTurn");

        public EndTurn(int id, IPlayer player) : base(id, player) { }
        public EndTurn(EndTurnData data, IRepository repo) : base(data, repo) { }

        public override ActionType ActionType => EndTurnType;
        public IPlayTurnsTurn NextTurn { get; private set; }

        public override bool IsAllowedInTurnPhase(ITurnPhase tp) => true;
        public override bool IsAllowedInGamePhase(IGamePhase gp) => gp.IsTurns;

        public override GameActionData ToData() =>
            base.ToData(new EndTurnData
            {
                GameActionType = GameActionTypeData.EndTurn
            });

        public override IValidationResult Validate(IGame game) => Validator.Valid;

        public override void PerformAtServer(IServerGame serverGame)
        {
            NextTurn = serverGame.GetNextTurn();
        }

        public override void Perform(IGame game)
        {
            var tokens = Player.Pieces.OfType<RoadBuilding.Token>().ToList();
            foreach (RoadBuilding.Token token in tokens)
            {
                token.RemoveFromPlayer(Player);
            }
            game.PlayTurns.MoveToNextTurn(NextTurn);

            base.Perform(game);
        }
    }
}
