using YouTown.Validation;

namespace YouTown.GameAction
{
    public class LooseCards : GameActionBase
    {
        public static ActionType LooseCardsType = new ActionType("LooseCards");

        public LooseCards(IPlayer player) : base(player) { }
        public LooseCards(int id, IPlayer player, IResourceList resourcesToLoose) : base(id, player)
        {
            ResourcesToLoose = resourcesToLoose;
        }
        public LooseCards(LooseCardsData data, IRepository repo) : base(data, repo)
        {
            ResourcesToLoose = data.ResourcesToLoose?.FromData();
        }

        public override ActionType ActionType => LooseCardsType;
        public IResourceList ResourcesToLoose { get; }

        public override bool IsAllowedInTurnPhase(ITurnPhase tp) => tp.IsDiceRoll;
        public override bool IsAllowedInGamePhase(IGamePhase gp) => gp.IsTurns;

        public override GameActionData ToData() =>
            base.ToData(new LooseCardsData
            {
                GameActionType = GameActionTypeData.LooseCards,
                ResourcesToLoose = ResourcesToLoose?.ToData()
            });

        public override IValidationResult Validate(IGame game) =>
            new ValidateAll()
                .WithObject<NotNull>(ResourcesToLoose)
                .With<HasResources, IResourceList, IResourceList>(Player.Hand, ResourcesToLoose)
                .With<LoosesCorrectAmount, IPlayer, int>(Player, ResourcesToLoose.Count)
                .Validate();

        public override void Perform(IGame game)
        {
            Player.LooseResourcesTo(game.Bank.Resources, ResourcesToLoose, null);

            base.Perform(game);
        }
    }
}
