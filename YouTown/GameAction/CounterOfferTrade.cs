using YouTown.Validation;

namespace YouTown.GameAction
{
    /// <summary>
    /// An opponent performs a counter offer, rejecting the player on turn's 
    /// <seealso cref="TradeOffer"/> and proposing his offer instead
    /// </summary>
    /// This action is originated from an opponent, not the player on turn. Thus,
    /// the <seealso cref="Player"/> is the opponent. 
    public class CounterOfferTrade : IGameAction
    {
        public static ActionType CounterOfferTradeType = new ActionType("CounterOfferTrade");
        public CounterOfferTrade(int id, IPlayer player, TradeOffer tradeOffer, IResourceList counterOffered, IResourceList counterRequested)
        {
            Id = id;
            Player = player;
            CounterOffered = counterOffered;
            CounterRequested = counterRequested;
            TradeOffer = tradeOffer;
        }

        public int Id { get; }
        public ActionType ActionType => CounterOfferTradeType;
        public IPlayer Player { get; }
        public ITurnPhase TurnPhase { get; private set; }
        public IGamePhase GamePhase { get; private set; }
        public ITurn Turn { get; private set; }
        public TradeOffer TradeOffer { get; }
        public IResourceList CounterOffered { get; }
        public IResourceList CounterRequested { get; }
        public bool IsAllowedInOpponentTurn => true;

        public bool IsAllowedInTurnPhase(ITurnPhase turnPhase) => turnPhase.IsTrading;
        public bool IsAllowedInGamePhase(IGamePhase gamePhase) => gamePhase.IsTurns;

        public IValidationResult Validate(IGame game) =>
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

        public void PerformAtServer(IServerGame serverGame)
        {
        }

        public void Perform(IGame game)
        {
            var counter = new CounterOffer(Player, CounterOffered, CounterRequested);
            TradeOffer.Responses.Add(counter);

            TurnPhase = game.PlayTurns.TurnPhase;
            GamePhase = game.GamePhase;
            Turn = game.PlayTurns.Turn;
        }
    }
}
