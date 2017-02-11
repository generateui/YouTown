using YouTown.Validation;

namespace YouTown.GameAction
{
    public class RejectTradeOffer : GameActionBase
    {
        public static ActionType RejectTradeOfferType = new ActionType("RejectTradeOffer");

        public RejectTradeOffer(int id, IPlayer player, TradeOffer tradeOffer) : base(id, player)
        {
            TradeOffer = tradeOffer;
        }

        public RejectTradeOffer(RejectTradeOfferData data, IRepository repo) : base(data, repo)
        {
            TradeOffer = repo.GetOrNull<TradeOffer>(data.TradeOfferId);
        }

        public override ActionType ActionType => RejectTradeOfferType;
        public TradeOffer TradeOffer { get; }
        public override bool IsAllowedInOpponentTurn => true;

        public override bool IsAllowedInTurnPhase(ITurnPhase turnPhase) => turnPhase.IsTrading;
        public override bool IsAllowedInGamePhase(IGamePhase gamePhase) => gamePhase.IsTurns;

        public override GameActionData ToData() =>
            base.ToData(new RejectTradeOfferData
            {
                GameActionType = GameActionTypeData.RejectTradeOffer,
                TradeOfferId = TradeOffer?.Id
            });

        public override IValidationResult Validate(IGame game) =>
            new ValidateAll()
                .WithObject<NotNull>(TradeOffer)
                .With<NotRespondedYet, IPlayer, TradeOffer>(Player, TradeOffer)
                .Validate();

        public override void Perform(IGame game)
        {
            var rejectOffer = new Reject(game.Identifier.NewId(), Player);
            game.Repository.Add(rejectOffer);
            TradeOffer.Responses.Add(rejectOffer);

            base.Perform(game);
        }
    }
}
