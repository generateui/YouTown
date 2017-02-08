using System;
using YouTown.Validation;

namespace YouTown.GameAction
{
    public class TradeWithBank : IGameAction
    {
        public static ActionType TradeWithBankType = new ActionType("TradeWithBank");
        public TradeWithBank(int id, IPlayer player, IResourceList offeredToBank, IResourceList requestedFromBank)
        {
            Id = id;
            Player = player;
            OfferedToBank = offeredToBank;
            RequestedFromBank = requestedFromBank;
        }

        public int Id { get; }
        public ActionType ActionType => TradeWithBankType;
        public IPlayer Player { get; }
        public ITurnPhase TurnPhase { get; private set; }
        public IGamePhase GamePhase { get; private set; }
        public ITurn Turn { get; private set; }
        public IResourceList OfferedToBank { get; }
        public IResourceList RequestedFromBank { get; }
        public bool IsAllowedInOpponentTurn => false;
        public bool IsAllowedInTurnPhase(ITurnPhase turnPhase) => turnPhase.IsTrading;
        public bool IsAllowedInGamePhase(IGamePhase gamePhase) => gamePhase.IsTurns;

        public IValidationResult Validate(IGame game) =>
            new ValidateAll()
                .WithObject<NotNull>(OfferedToBank)
                .WithObject<NotEmpty>(OfferedToBank)
                .WithObject<NotNull>(RequestedFromBank)
                .WithObject<NotEmpty>(RequestedFromBank)
                .With<HasResources, IResourceList, IResourceList>(Player.Hand, OfferedToBank, Player.User.Name)
                .With<HasResources, IResourceList, IResourceList>(game.Bank.Resources, RequestedFromBank, "bank")
                .With<IsCorrectBankTrade, Tuple<IPortList, IResourceList, IResourceList>>(
                    Tuple.Create(Player.Ports, OfferedToBank, RequestedFromBank))
                .Validate();

        public void PerformAtServer(IServerGame serverGame)
        {
        }

        public void Perform(IGame game)
        {
            Player.LooseResourcesTo(game.Bank.Resources, OfferedToBank, null);
            Player.GainResourcesFrom(game.Bank.Resources, RequestedFromBank, null);

            TurnPhase = game.PlayTurns.TurnPhase;
            GamePhase = game.GamePhase;
            Turn = game.PlayTurns.Turn;
        }
    }
}
