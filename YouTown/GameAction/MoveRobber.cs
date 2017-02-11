using YouTown.Validation;

namespace YouTown.GameAction
{
    public class MoveRobber : GameActionBase
    {
        public static ActionType MoveRobberType = new ActionType("MoveRobber");

        public MoveRobber(IPlayer player) : base(player) { }
        public MoveRobber(int id, IPlayer player) : base(id, player) { }

        public MoveRobber(MoveRobberData data, IRepository repo) : base(data, repo)
        {
            Location = data.Location != null ? new Location(data.Location) : null;
        }

        public override ActionType ActionType => MoveRobberType;
        public Location Location { get; set; }

        public override bool IsAllowedInTurnPhase(ITurnPhase tp) => tp.IsBeforeDiceRoll || tp.IsDiceRoll || tp.IsBuilding;
        public override bool IsAllowedInGamePhase(IGamePhase gp) => gp.IsTurns;

        public override GameActionData ToData() =>
            base.ToData(new MoveRobberData
            {
                GameActionType = GameActionTypeData.MoveRobber,
                Location = Location?.ToData()
            });

        public override IValidationResult Validate(IGame game) =>
            new ValidateAll()
                .WithObject<NotNull>(Location)
                .With<HasHexAt, IBoardForPlay, Location>(game.Board, Location)
                .With<CanPlaceRobberAt, IBoardForPlay, Location>(game.Board, Location)
                .Validate();

        public override void Perform(IGame game)
        {
            game.Board.Robber.Location = Location;

            base.Perform(game);
        }
    }
}
