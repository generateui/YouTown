using YouTown.Validation;

namespace YouTown.GameAction
{
    /// <summary>
    /// An opponent performs a counter offer, rejecting the player on turn's 
    /// <seealso cref="TradeOffer"/> and proposing his offer instead
    /// </summary>
    /// This action is originated from an opponent, not the player on turn. Thus,
    /// the <seealso cref="Player"/> is the opponent. 
    public class CounterOfferTrade : GameActionBase, IGameAction
    {
        public static ActionType CounterOfferTradeType = new ActionType("CounterOfferTrade");

        public CounterOfferTrade(int id, IPlayer player, TradeOffer tradeOffer, IResourceList counterOffered, IResourceList counterRequested) : base(id, player)
        {
            CounterOffered = counterOffered;
            CounterRequested = counterRequested;
            TradeOffer = tradeOffer;
        }
        public CounterOfferTrade(CounterOfferTradeData data, IRepository repo) : base(data, repo)
        {
            TradeOffer = repo.GetOrNull<TradeOffer>(data.TradeOfferId);
            CounterOffered = data.CounterOffered?.FromData();
            CounterRequested = data.CounterRequested.FromData();
        }

        public override ActionType ActionType => CounterOfferTradeType;
        public TradeOffer TradeOffer { get; }
        public IResourceList CounterOffered { get; }
        public IResourceList CounterRequested { get; }
        public override bool IsAllowedInOpponentTurn => true;

        public override bool IsAllowedInTurnPhase(ITurnPhase turnPhase) => turnPhase.IsTrading;
        public override bool IsAllowedInGamePhase(IGamePhase gamePhase) => gamePhase.IsTurns;

        public override GameActionData ToData() =>
            base.ToData(new CounterOfferTradeData
            {
                GameActionType = GameActionTypeData.CounterTradeOffer,
                TradeOfferId = TradeOffer?.Id,
                CounterOffered = CounterOffered?.ToData(),
                CounterRequested = CounterRequested?.ToData()
            });

        public override IValidationResult Validate(IGame game) =>
            new ValidateAll()
                .WithObject<NotNull>(TradeOffer)
                .WithObject<NotNull>(CounterOffered)
                .WithObject<NotNull>(CounterRequested)
                .WithObject<NotEmpty>(CounterOffered)
                .WithObject<NotEmpty>(CounterRequested)
                .With<HasResourceAmount, IPlayer, int>(Player, CounterRequested.Count)
                .With<HasResources, IResourceList, IResourceList>(Player.Hand, CounterOffered)
                .With<NotRespondedYet, IPlayer, TradeOffer>(Player, TradeOffer)
                .Validate();

        public override void Perform(IGame game)
        {
            var id = game.Identifier.NewId();
            var counter = new CounterOffer(id, Player, CounterOffered, CounterRequested);
            game.Repository.Add(counter);
            TradeOffer.Responses.Add(counter);

            base.Perform(game);
        }
    }
}
