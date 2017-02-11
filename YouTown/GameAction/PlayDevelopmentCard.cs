using Bond;
using YouTown.Validation;

namespace YouTown.GameAction
{
    public class PlayDevelopmentCard : GameActionBase
    {
        public static ActionType PlayDevelopmentCardType = new ActionType("PlayDevelopmentCard");

        public PlayDevelopmentCard(int id, IPlayer player, IDevelopmentCard developmentCard) : base(id, player)
        {
            DevelopmentCard = developmentCard;
        }
        public PlayDevelopmentCard(PlayDevelopmentCardData data, IRepository repo) : base(data, repo)
        {
            DevelopmentCard = data.DevelopmentCard?.FromData(repo);
        }

        public override ActionType ActionType => PlayDevelopmentCardType;
        public IDevelopmentCard DevelopmentCard { get;  }

        public override bool IsAllowedInTurnPhase(ITurnPhase tp) => tp.IsDiceRoll || tp.IsTrading;
        public override bool IsAllowedInGamePhase(IGamePhase gp) => gp.IsTurns;

        public override GameActionData ToData() =>
            base.ToData(new PlayDevelopmentCardData
            {
                GameActionType = GameActionTypeData.PlayDevelopmentCard,
                DevelopmentCard = DevelopmentCard != null ? new Bonded<DevelopmentCardData>(DevelopmentCard.ToData()) : null,
            });

        public override IValidationResult Validate(IGame game) =>
            new ValidateAll()
                .WithObject<NotNull>(DevelopmentCard)
                .With<IsOnTurn, IPlayer>(Player)
                .With<WaitedOneTurn, IDevelopmentCard, ITurn>(DevelopmentCard, game.PlayTurns.Turn)
                .With<NotYetPlayedDevelopmentCard, IPlayTurnsTurn, IDevelopmentCard>(game.PlayTurns.Turn, DevelopmentCard)
                .Validate();

        public override void PerformAtServer(IServerGame serverGame)
        {
            serverGame.DevelopmentCardsByPlayer[Player].Remove(DevelopmentCard);
        }

        public override void Perform(IGame game)
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

            base.Perform(game);
        }
    }
}
