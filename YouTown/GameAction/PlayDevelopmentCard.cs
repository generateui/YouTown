using YouTown.Validator;

namespace YouTown.GameAction
{
    public class PlayDevelopmentCard : IGameAction
    {
        public PlayDevelopmentCard(int id, IPlayer player, IDevelopmentCard developmentCard)
        {
            Id = id;
            Player = player;
            DevelopmentCard = developmentCard;
        }

        public int Id { get; }
        public IPlayer Player { get; }
        public ITurnPhase TurnPhase { get; private set; }
        public IGamePhase GamePhase { get; private set; }
        public ITurn Turn { get; private set; }
        public IDevelopmentCard DevelopmentCard { get;  }
        public bool IsAllowedInOpponentTurn => false;

        public bool IsAllowedInTurnPhase(ITurnPhase tp) => tp.IsDiceRoll || tp.IsTrading;
        public bool IsAllowedInGamePhase(IGamePhase gp) => gp.IsTurns;

        public IValidationResult Validate(IGame game) =>
            new ValidateAll()
                .WithObject<NotNull>(DevelopmentCard)
                .With<IsOnTurn, IPlayer>(Player)
                .With<WaitedOneTurn, IDevelopmentCard, ITurn>(DevelopmentCard, game.PlayTurns.Turn)
                .With<NotYetPlayedDevelopmentCard, ITurn, IDevelopmentCard>(game.PlayTurns.Turn, DevelopmentCard)
                .Validate();

        public void PerformAtServer(IServerGame serverGame)
        {
            serverGame.DevelopmentCardsByPlayer[Player].Remove(DevelopmentCard);
        }

        public void Perform(IGame game)
        {
            var turn = game.PlayTurns.Turn;
            DevelopmentCard.Play(game);
            DevelopmentCard.TurnPlayed = turn;
            DevelopmentCard.RemoveFromPlayer(Player);
            Player.PlayedDevelopmentCards.Add(DevelopmentCard);
            if (DevelopmentCard.MaxOnePerTurn)
            {
                turn.HasPlayedDevelopmentCard = true;
            }

            TurnPhase = game.PlayTurns.TurnPhase;
            GamePhase = game.GamePhase;
            Turn = turn;
        }
    }
}
