using YouTown.Validation;

namespace YouTown.GameAction
{
    public class RollDice : GameActionBase
    {
        public static ActionType RollDiceType = new ActionType("RollDice");

        public RollDice(int id, IPlayer player) : base(id, player) { }
        public RollDice(RollDiceData data, IRepository repo) : base(data, repo)
        {
            DiceRoll = data.RolledDice != null ? new DiceRoll(data.RolledDice) : null;
            Production = data.Production != null ? new Production(data.Production, repo) : null;
        }

        public override ActionType ActionType => RollDiceType;
        public DiceRoll DiceRoll { get; private set; }
        public Production Production { get; private set; }

        public override bool IsAllowedInTurnPhase(ITurnPhase turnPhase) => turnPhase.IsDiceRoll;
        public override bool IsAllowedInGamePhase(IGamePhase gamePhase) => gamePhase.IsTurns; // TODO: determinefirstplayer

        public override GameActionData ToData() =>
            base.ToData(new RollDiceData
            {
                GameActionType = GameActionTypeData.RollDice,
                Production = Production?.ToData(),
            });

        public override IValidationResult Validate(IGame game) =>
            new ValidateAll()
                .WithObject<NotNull>(DiceRoll)
                .Validate();

        public override void PerformAtServer(IServerGame serverGame)
        {
            var random = serverGame.Random;
            var die1 = random.NextInt(1, 6);
            var die2 = random.NextInt(1, 6);
            DiceRoll = new DiceRoll(die1, die2);
        }

        public override void Perform(IGame game)
        {
            Production = game.GamePhase.RollDice(game, DiceRoll, Player);

            base.Perform(game);
        }
    }
}
