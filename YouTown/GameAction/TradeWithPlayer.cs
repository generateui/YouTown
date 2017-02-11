using YouTown.Validation;

namespace YouTown.GameAction
{
    public class TradeWithPlayer : GameActionBase
    {
        public static ActionType TradeWithPlayerType = new ActionType("TradeWithPlayer");

        // TODO: fix this ridiculously long list
        public TradeWithPlayer(
            int id,
            IPlayer player,
            IResourceList offered,
            IResourceList requested,
            IPlayer opponent,
            TradeOffer tradeOffer) : base(id, player)
        {
            Offered = offered;
            Requested = requested;
            Opponent = opponent;
            TradeOffer = tradeOffer;
        }
        public TradeWithPlayer(TradeWithPlayerData data, IRepository repo) : base(data, repo)
        {
            Offered = data.Offered?.FromData();
            Requested = data.Requested?.FromData();
            Opponent = repo.GetOrNull<IPlayer>(data.OpponentId);
            TradeOffer = repo.GetOrNull<TradeOffer>(data.TradeOfferId);
            Accept = repo.GetOrNull<Accept>(data.AcceptId);
        }

        public override ActionType ActionType => TradeWithPlayerType;
        public IResourceList Offered { get; }
        public IResourceList Requested { get; }
        public IPlayer Opponent { get; }
        public TradeOffer TradeOffer { get; }
        public Accept Accept { get; set; }

        public override bool IsAllowedInTurnPhase(ITurnPhase turnPhase) => turnPhase.IsTrading;
        public override bool IsAllowedInGamePhase(IGamePhase gamePhase) => gamePhase.IsTurns;

        public override GameActionData ToData() =>
            ToData(new TradeWithPlayerData
            {
                GameActionType = GameActionTypeData.TradeWithPlayer,
                Offered = Offered?.ToData(),
                Requested = Requested?.ToData(),
                OpponentId = Opponent?.Id,
                TradeOfferId = TradeOffer?.Id,
                AcceptId = Accept.Id,
            });

        public override IValidationResult Validate(IGame game) =>
            new ValidateAll()
                .WithObject<NotNull>(Offered)
                .WithObject<NotNull>(Requested)
                .WithObject<NotEmpty>(Offered)
                .WithObject<NotEmpty>(Requested)
                .With<HasResources, IResourceList, IResourceList>(Player.Hand, Offered)
                .With<HasResources, IResourceList, IResourceList>(Opponent.Hand, Requested)
                .With<HasResponse, TradeOffer, Accept>(TradeOffer, Accept)
                .Validate();

        public override void Perform(IGame game)
        {
            Player.GainResourcesFrom(Offered, Requested, null);

            base.Perform(game);
        }
    }
}
