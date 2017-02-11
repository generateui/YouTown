using YouTown.Validation;

namespace YouTown.GameAction
{
    public class OfferTrade : GameActionBase
    {
        public static ActionType OfferTradeType = new ActionType("OfferTrade");

        public OfferTrade(int id, IPlayer player, IResourceList offered, IResourceList requested) : base(id, player)
        {
            Offered = offered;
            Requested = requested;
        }
        public OfferTrade(OfferTradeData data, IRepository repo) : base(data, repo)
        {
            Offered = data.Offered.FromData();
            Requested = data.Requested.FromData();
        }

        public override ActionType ActionType => OfferTradeType;
        public IResourceList Offered { get; }
        public IResourceList Requested { get; }

        public override bool IsAllowedInTurnPhase(ITurnPhase turnPhase) => turnPhase.IsTrading;
        public override bool IsAllowedInGamePhase(IGamePhase gamePhase) => gamePhase.IsTurns;

        public override GameActionData ToData() =>
            base.ToData(new OfferTradeData
            {
                GameActionType = GameActionTypeData.OfferTrade,
                Offered = Offered?.ToData(),
                Requested = Requested?.ToData(),
            });

        // TODO: should we check for all other players to have any resource? I have been there...
        public override IValidationResult Validate(IGame game) =>
            new ValidateAll()
                .WithObject<NotNull>(Offered)
                .WithObject<NotNull>(Requested)
                .WithObject<NotEmpty>(Offered)
                .WithObject<NotEmpty>(Requested)
                .With<HasResources, IResourceList, IResourceList>(Player.Hand, Offered)
                .Validate();

        public override void Perform(IGame game)
        {
            var id = game.Identifier.NewId();
            var tradeOffer = new TradeOffer(id, Player, Offered, Requested);
            game.Repository.Add(tradeOffer);
            game.PlayTurns.Turn.TradeOffers.Add(tradeOffer);

            base.Perform(game);
        }
    }
}
