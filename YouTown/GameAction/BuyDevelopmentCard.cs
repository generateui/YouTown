using System;
using System.Linq;
using YouTown.Validator;

namespace YouTown.GameAction
{
    public class BuyDevelopmentCard : IGameAction, IObscurable
    {
        public BuyDevelopmentCard(int id, IPlayer player)
        {
            Id = id;
            Player = player;
        }

        public int Id { get; }
        public IPlayer Player { get; }
        public ITurnPhase TurnPhase { get; private set; }
        public IGamePhase GamePhase { get; private set; }
        public ITurn Turn { get; private set; }
        public bool IsAllowedInOpponentTurn => false;
        public bool IsAtServer { get; }
        public IPlayer PlayerAtClient { get; }
        public IDevelopmentCard DevelopmentCard { get; set; }

        public bool IsAllowedInTurnPhase(ITurnPhase tp) => tp.IsBuilding;
        public bool IsAllowedInGamePhase(IGamePhase gp) => gp.IsTurns;

        public IValidationResult Validate(IGame game) =>
            new ValidateAll()
                .With<IsOnTurn, IPlayer>(Player)
                .WithObject<NotEmpty>(game.Bank.DevelopmentCards, "bank development cards")
                .With<CanPayPiece, IPlayer, IPiece>(Player, DevelopmentCard)
                .Validate();

        public void PerformAtServer(IServerGame serverGame)
        {
            var developmentcard = serverGame.DevelopmentCards.Last();
            serverGame.DevelopmentCards.Remove(developmentcard);
            serverGame.DevelopmentCardsByPlayer[Player].Add(developmentcard);
        }

        public void Perform(IGame game)
        {
            var developmentCard = game.Bank.DevelopmentCards.Last();
            game.Bank.DevelopmentCards.Remove(developmentCard);
            developmentCard.AddToPlayer(Player);
            IResourceList cost = new DevelopmentCardCost();
            Player.LooseResourcesTo(game.Bank.Resources, cost, this);
            developmentCard.TurnBought = game.PlayTurns.Turn;

            GamePhase = game.GamePhase;
            Turn = game.PlayTurns.Turn;
            TurnPhase = game.PlayTurns.TurnPhase;
        }

    }
}
