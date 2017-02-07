using YouTown.Validation;

namespace YouTown.GameAction
{
    public class RollDice : IGameAction
    {
        public RollDice(int id, IPlayer player)
        {
            Id = id;
            Player = player;
        }

        public int Id { get; }
        public IPlayer Player { get; }
        public ITurnPhase TurnPhase { get; private set; }
        public IGamePhase GamePhase { get; private set; }
        public ITurn Turn { get; private set; }
        public DiceRoll DiceRoll { get; private set; }
        public bool IsAllowedInOpponentTurn => false;
        public Production Production { get; private set; }

        public bool IsAllowedInTurnPhase(ITurnPhase turnPhase) => turnPhase.IsDiceRoll;
        public bool IsAllowedInGamePhase(IGamePhase gamePhase) => gamePhase.IsTurns; // TODO: determinefirstplayer

        public IValidationResult Validate(IGame game) =>
            new ValidateAll()
                .WithObject<NotNull>(DiceRoll)
                .Validate();

        public void PerformAtServer(IServerGame serverGame)
        {
            var random = serverGame.Random;
            var die1 = random.NextInt(1, 6);
            var die2 = random.NextInt(1, 6);
            DiceRoll = new DiceRoll(die1, die2);
        }

        public void Perform(IGame game)
        {
            Production = game.GamePhase.RollDice(game, DiceRoll, Player);

            TurnPhase = game.PlayTurns.TurnPhase;
            GamePhase = game.GamePhase;
            Turn = game.PlayTurns.Turn;
        }
    }
}
