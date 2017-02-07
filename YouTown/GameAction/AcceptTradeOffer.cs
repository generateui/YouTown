﻿using YouTown.Validation;

namespace YouTown.GameAction
{
    public class AcceptTradeOffer : IGameAction
    {
        public AcceptTradeOffer(int id, IPlayer player, TradeOffer tradeOffer)
        {
            Id = id;
            Player = player;
            TradeOffer = tradeOffer;
        }

        public int Id { get; }
        public IPlayer Player { get; }
        public ITurnPhase TurnPhase { get; private set; }
        public IGamePhase GamePhase { get; private set; }
        public ITurn Turn { get; private set; }
        public TradeOffer TradeOffer { get; }
        public bool IsAllowedInOpponentTurn => true;

        public bool IsAllowedInTurnPhase(ITurnPhase turnPhase) => turnPhase.IsTrading;
        public bool IsAllowedInGamePhase(IGamePhase gamePhase) => gamePhase.IsTurns;

        public IValidationResult Validate(IGame game) =>
            new ValidateAll()
                .WithObject<NotNull>(TradeOffer)
                .With<NotRespondedYet, IPlayer, TradeOffer>(Player, TradeOffer)
                .Validate();

        public void PerformAtServer(IServerGame serverGame)
        {
        }

        public void Perform(IGame game)
        {
            var accept = new Accept(Player);
            TradeOffer.Responses.Add(accept);

            TurnPhase = game.PlayTurns.TurnPhase;
            GamePhase = game.GamePhase;
            Turn = game.PlayTurns.Turn;
        }
}
}