using YouTown.Validation;

namespace YouTown.GameAction
{
    public class RejectTradeOffer : IGameAction
    {
        public static ActionType RejectTradeOfferType = new ActionType("RejectTradeOffer");
        public RejectTradeOffer(int id, IPlayer player, TradeOffer tradeOffer)
        {
            Id = id;
            Player = player;
            TradeOffer = tradeOffer;
        }

        public int Id { get; }
        public ActionType ActionType => RejectTradeOfferType;
        public IPlayer Player { get; }
        public ITurnPhase TurnPhase { get; private set; }
        public IGamePhase GamePhase { get; private set; }
        public ITurn Turn { get; private set; }
        public TradeOffer TradeOffer { get; }
        public bool IsAllowedInOpponentTurn => true;

        public bool IsAllowedInTurnPhase(ITurnPhase turnPhase) => turnPhase.IsTrading;
        public bool IsAllowedInGamePhase(IGamePhase gamePhase) => gamePhase.IsTurns;

        public IValidationResult Validate(IGame game) =>
            new ValidateAll()
                .WithObject<NotNull>(TradeOffer)
                .With<NotRespondedYet, IPlayer, TradeOffer>(Player, TradeOffer)
                .Validate();

        public void PerformAtServer(IServerGame serverGame)
        {
        }

        public void Perform(IGame game)
        {
            var rejectOffer = new Reject(Player);
            TradeOffer.Responses.Add(rejectOffer);

            TurnPhase = game.PlayTurns.TurnPhase;
            GamePhase = game.GamePhase;
            Turn = game.PlayTurns.Turn;
        }
    }
}
