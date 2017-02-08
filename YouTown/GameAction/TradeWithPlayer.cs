using YouTown.Validation;

namespace YouTown.GameAction
{
    public class TradeWithPlayer : IGameAction
    {
        public static ActionType TradeWithPlayerType = new ActionType("TradeWithPlayer");
        // TODO: fix this ridiculously long list
        public TradeWithPlayer(
            int id,
            IPlayer player,
            IResourceList offered,
            IResourceList requested,
            IPlayer opponent,
            TradeOffer tradeOffer)
        {
            Id = id;
            Player = player;
            Offered = offered;
            Requested = requested;
            Opponent = opponent;
            TradeOffer = tradeOffer;
        }

        public int Id { get; }
        public ActionType ActionType => TradeWithPlayerType;
        public IPlayer Player { get; }
        public ITurnPhase TurnPhase { get; private set; }
        public IGamePhase GamePhase { get; private set; }
        public ITurn Turn { get; private set; }
        public IResourceList Offered { get; }
        public IResourceList Requested { get; }
        public IPlayer Opponent { get; }
        public TradeOffer TradeOffer { get; }
        public Accept Accept { get; set; }
        public bool IsAllowedInOpponentTurn => false;

        public bool IsAllowedInTurnPhase(ITurnPhase turnPhase) => turnPhase.IsTrading;
        public bool IsAllowedInGamePhase(IGamePhase gamePhase) => gamePhase.IsTurns;

        public IValidationResult Validate(IGame game) =>
            new ValidateAll()
                .WithObject<NotNull>(Offered)
                .WithObject<NotNull>(Requested)
                .WithObject<NotEmpty>(Offered)
                .WithObject<NotEmpty>(Requested)
                .With<HasResources, IResourceList, IResourceList>(Player.Hand, Offered)
                .With<HasResources, IResourceList, IResourceList>(Opponent.Hand, Requested)
                .With<HasResponse, TradeOffer, Accept>(TradeOffer, Accept)
                .Validate();

        public void PerformAtServer(IServerGame serverGame)
        {
        }

        public void Perform(IGame game)
        {
            Player.GainResourcesFrom(Offered, Requested, null);

            TurnPhase = game.PlayTurns.TurnPhase;
            GamePhase = game.GamePhase;
            Turn = game.PlayTurns.Turn;
        }
    }
}
