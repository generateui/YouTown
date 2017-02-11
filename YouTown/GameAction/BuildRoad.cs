using System.Linq;
using YouTown.Validation;

namespace YouTown.GameAction
{
    public class BuildRoad : GameActionBase
    {
        public static ActionType BuildRoadType = new ActionType("BuildRoad");
        public BuildRoad(int id, IPlayer player, Edge edge) : base(id, player)
        {
            Edge = edge;
        }

        public BuildRoad(BuildRoadData data, IRepository repo): base(data, repo)
        {
            Edge = data.Edge != null ? new Edge(data.Edge) : null;
        }

        public override ActionType ActionType => BuildRoadType;
        public Edge Edge { get; }

        public override bool IsAllowedInTurnPhase(ITurnPhase tp) => tp.IsBuilding;
        public override bool IsAllowedInGamePhase(IGamePhase gp) => gp.IsTurns || gp.IsInitialPlacement;

        public override GameActionData ToData() =>
            base.ToData(new BuildRoadData
            {
                GameActionType = GameActionTypeData.BuildRoad,
                Edge = Edge?.ToData()
            });

        public override IValidationResult Validate(IGame game) =>
            BaseValidate(game)
                .WithObject<NotNull>(Edge)
                .With<HasRoadInStock, IPlayer>(Player)
                //                .With<RoadAllowedWhenInitialPlacement>(game.GamePhase) TODO: implement
                .With<IsOnTurn, IPlayer>(Player)
                .With<CanBuildRoadAt, Edge, IBoardForPlay>(Edge, game.Board)
                .With<HasConnectionToEdge, Edge, IPlayer>(Edge, Player)
                // .With<HasLandHex, Edge, IBoard>
                .Validate();


        public override void Perform(IGame game)
        {
            var road = Player.Stock[Road.RoadType].Last() as Road;
            road.Edge = Edge;
            road.AddToPlayer(Player);
            road.AddToBoard(game.Board);
            game.GamePhase.BuildRoad(game, Player, road);
            //            game.MoveLongestRoadIfNeeded(); TODO: implement

            base.Perform(game);
        }
    }
}
