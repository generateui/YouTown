using YouTown.Validation;

namespace YouTown.GameAction
{
    public class OfferTrade : IGameAction
    {
        public OfferTrade(int id, IPlayer player, IResourceList offered, IResourceList requested)
        {
            Id = id;
            Player = player;
            Offered = offered;
            Requested = requested;
        }

        public int Id { get; }
        public IPlayer Player { get; }
        public ITurnPhase TurnPhase { get; private set; }
        public IGamePhase GamePhase { get; private set; }
        public ITurn Turn { get; private set; }
        public IResourceList Offered { get; }
        public IResourceList Requested { get; }
        public bool IsAllowedInOpponentTurn => false;

        public bool IsAllowedInTurnPhase(ITurnPhase turnPhase) => turnPhase.IsTrading;
        public bool IsAllowedInGamePhase(IGamePhase gamePhase) => gamePhase.IsTurns;

        // TODO: should we check for all other players to have any resource? I have been there...
        public IValidationResult Validate(IGame game) =>
            new ValidateAll()
                .WithObject<NotNull>(Offered)
                .WithObject<NotNull>(Requested)
                .WithObject<NotEmpty>(Offered)
                .WithObject<NotEmpty>(Requested)
                .With<HasResources, IResourceList, IResourceList>(Player.Hand, Offered)
                .Validate();

        public void PerformAtServer(IServerGame serverGame)
        {
        }

        public void Perform(IGame game)
        {
            var tradeOffer = new TradeOffer(Player, Offered, Requested);
            game.PlayTurns.Turn.TradeOffers.Add(tradeOffer);

            TurnPhase = game.PlayTurns.TurnPhase;
            GamePhase = game.GamePhase;
            Turn = game.PlayTurns.Turn;
        }
    }
}
