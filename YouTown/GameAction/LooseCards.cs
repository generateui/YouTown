namespace YouTown.GameAction
{
    public class LooseCards : IGameAction
    {
        public LooseCards(int id, IPlayer player, IResourceList resourcesToLoose)
        {
            Id = id;
            Player = player;
            ResourcesToLoose = resourcesToLoose;
        }

        public int Id { get; }
        public IPlayer Player { get; }
        public ITurnPhase TurnPhase { get; private set; }
        public IGamePhase GamePhase { get; private set; }
        public ITurn Turn { get; private set; }
        public IResourceList ResourcesToLoose { get; }
        public bool IsAllowedInOpponentTurn => false;

        public bool IsAllowedInTurnPhase(ITurnPhase tp) => tp.IsDiceRoll;
        public bool IsAllowedInGamePhase(IGamePhase gp) => gp.IsTurns;

        public IValidationResult Validate(IGame game) =>
            new ValidateAll()
                .WithObject<NotNull>(ResourcesToLoose)
                .With<HasResources, IPlayer, IResourceList>(Player, ResourcesToLoose)
                .With<LoosesCorrectAmount, IPlayer, int>(Player, ResourcesToLoose.Count)
                .Validate();

        public void PerformAtServer(IServerGame serverGame)
        {
        }

        public void Perform(IGame game)
        {
            Player.LooseResourcesTo(game.Bank.Resources, ResourcesToLoose, null);

            TurnPhase = game.PlayTurns.TurnPhase;
            GamePhase = game.GamePhase;
            Turn = game.PlayTurns.Turn;
        }
    }
}
