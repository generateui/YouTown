using YouTown.Validation;

namespace YouTown.GameAction
{
    public class AcceptTradeOffer : GameActionBase, IGameAction
    {
        public static ActionType AcceptTradeOfferType = new ActionType("AcceptTradeOffer");

        public AcceptTradeOffer(int id, IPlayer player, TradeOffer tradeOffer) : base (id, player)
        {
            TradeOffer = tradeOffer;
        }

        public AcceptTradeOffer(AcceptTradeOfferData data, IRepository repo) : base(data, repo)
        {
            TradeOffer = repo.GetOrNull<TradeOffer>(data.TradeOfferId);
        }

        public override ActionType ActionType => AcceptTradeOfferType;
        public TradeOffer TradeOffer { get; }
        public override bool IsAllowedInOpponentTurn => true;

        public override bool IsAllowedInTurnPhase(ITurnPhase turnPhase) => turnPhase.IsTrading;
        public override bool IsAllowedInGamePhase(IGamePhase gamePhase) => gamePhase.IsTurns;

        public override GameActionData ToData()
        {
            var data = new AcceptTradeOfferData
            {
                GameActionType = GameActionTypeData.AcceptTradeOffer,
                TradeOfferId = TradeOffer?.Id
            };
            return base.ToData(data);
        }

        public override IValidationResult Validate(IGame game) =>
            BaseValidate(game)
                .WithObject<NotNull>(TradeOffer)
                .With<NotRespondedYet, IPlayer, TradeOffer>(Player, TradeOffer)
                .Validate();

        public override void Perform(IGame game)
        {
            var accept = new Accept(game.Identifier.NewId(), Player);
            game.Repository.Add(accept);
            TradeOffer.Responses.Add(accept);

            base.Perform(game);
        }
    }
}
