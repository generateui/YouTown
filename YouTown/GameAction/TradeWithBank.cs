using System;
using YouTown.Validation;

namespace YouTown.GameAction
{
    public class TradeWithBank : GameActionBase
    {
        public static ActionType TradeWithBankType = new ActionType("TradeWithBank");
        public TradeWithBank(int id, IPlayer player, IResourceList offered, IResourceList requested) : base(id, player)
        {
            Offered = offered;
            Requested = requested;
        }

        public TradeWithBank(TradeWithBankData data, IRepository repo) : base(data, repo)
        {
            Offered = data.Offered?.FromData();
            Requested = data.Requested?.FromData();
        }

        public override ActionType ActionType => TradeWithBankType;
        public IResourceList Offered { get; }
        public IResourceList Requested { get; }

        public override bool IsAllowedInTurnPhase(ITurnPhase turnPhase) => turnPhase.IsTrading;
        public override bool IsAllowedInGamePhase(IGamePhase gamePhase) => gamePhase.IsTurns;

        public override GameActionData ToData() =>
            ToData(new TradeWithBankData
            {
                GameActionType = GameActionTypeData.TradeWithBank,
                Offered = Offered?.ToData(),
                Requested = Requested?.ToData(),
            });

        public override IValidationResult Validate(IGame game) =>
            new ValidateAll()
                .WithObject<NotNull>(Offered)
                .WithObject<NotEmpty>(Offered)
                .WithObject<NotNull>(Requested)
                .WithObject<NotEmpty>(Requested)
                .With<HasResources, IResourceList, IResourceList>(Player.Hand, Offered, Player.User.Name)
                .With<HasResources, IResourceList, IResourceList>(game.Bank.Resources, Requested, "bank")
                .With<IsCorrectBankTrade, Tuple<IPortList, IResourceList, IResourceList>>(
                    Tuple.Create(Player.Ports, Offered, Requested))
                .Validate();

        public override void Perform(IGame game)
        {
            Player.LooseResourcesTo(game.Bank.Resources, Offered, null);
            Player.GainResourcesFrom(game.Bank.Resources, Requested, null);

            base.Perform(game);
        }
    }
}
