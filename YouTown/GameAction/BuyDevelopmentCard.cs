using System.Linq;
using Bond;
using YouTown.Validation;

namespace YouTown.GameAction
{
    public class BuyDevelopmentCard : GameActionBase, IObscurable
    {
        public static ActionType BuyDevelopmentCardType = new ActionType("BuyDevelopmentCard");

        public BuyDevelopmentCard(int id, IPlayer player) : base(id, player) { }

        public BuyDevelopmentCard(BuyDevelopmentCardData data, IRepository repo) : base(data, repo)
        {
            DevelopmentCard = data.DevelopmentCard?.FromData(repo);
        }

        public override ActionType ActionType => BuyDevelopmentCardType;
        public bool IsAtServer { get; }
        public IPlayer PlayerAtClient { get; }
        public IDevelopmentCard DevelopmentCard { get; set; }

        public override bool IsAllowedInTurnPhase(ITurnPhase tp) => tp.IsBuilding;
        public override bool IsAllowedInGamePhase(IGamePhase gp) => gp.IsTurns;

        public override GameActionData ToData() =>
            base.ToData(new BuyDevelopmentCardData
            {
                GameActionType = GameActionTypeData.BuyDevelopmentCard,
                DevelopmentCard = DevelopmentCard != null ? new Bonded<DevelopmentCardData>(DevelopmentCard.ToData()) : null,
            });

        public override IValidationResult Validate(IGame game) =>
            new ValidateAll()
                .With<IsOnTurn, IPlayer>(Player)
                .WithObject<NotEmpty>(game.Bank.DevelopmentCards, "bank development cards")
                .With<CanPayPiece, IPlayer, IPiece>(Player, DevelopmentCard)
                .Validate();

        public override void PerformAtServer(IServerGame serverGame)
        {
            var developmentcard = serverGame.DevelopmentCards.Last();
            serverGame.DevelopmentCards.Remove(developmentcard);
            serverGame.DevelopmentCardsByPlayer[Player].Add(developmentcard);
        }

        public override void Perform(IGame game)
        {
            var developmentCard = game.Bank.DevelopmentCards.Last();
            game.Bank.DevelopmentCards.Remove(developmentCard);
            developmentCard.AddToPlayer(Player);
            IResourceList cost = new DevelopmentCardCost();
            Player.LooseResourcesTo(game.Bank.Resources, cost, this);
            developmentCard.TurnBought = game.PlayTurns.Turn;

            base.Perform(game);
        }

    }
}
