using System.Linq;
using YouTown.Validation;

namespace YouTown.GameAction
{
    public class BuildTown : GameActionBase
    {
        public static ActionType BuildTownType = new ActionType("BuildTown");
        public BuildTown(int id, IPlayer player, Vertex vertex) : base (id, player)
        {
            Vertex = vertex;
        }

        public BuildTown(BuildTownData data, IRepository repo) : base(data, repo)
        {
            Vertex = data.Vertex != null ? new Vertex(data.Vertex) : null;
        }

        public override ActionType ActionType => BuildTownType;
        public Vertex Vertex { get; }

        public override bool IsAllowedInTurnPhase(ITurnPhase tp) => tp.IsBuilding;
        public override bool IsAllowedInGamePhase(IGamePhase gp) => gp.IsTurns || gp.IsInitialPlacement;

        public override GameActionData ToData() =>
            base.ToData(new BuildTownData
            {
                GameActionType = GameActionTypeData.BuildTown,
                Vertex = Vertex?.ToData()
            });

        public override IValidationResult Validate(IGame game) =>
            new ValidateAll()
                .WithObject<NotNull>(Vertex)
                .With<HasTownInStock, IPlayer>(Player)
//                .With<TownAllowedWhenInitialPlacement>(game.GamePhase) TODO: implement
                .With<IsOnTurn, IPlayer>(Player)
                .With<CanBuildTownAt, Vertex, IBoardForPlay>(Vertex, game.Board)
                .With<HasRoadToVertex, Vertex, IPlayer>(Vertex, Player)
                .Validate();

        public override void Perform(IGame game)
        {
            var town = Player.Stock[Town.TownType].Last() as Town;
            game.GamePhase.BuildTown(game, Player, town);
            town.Vertex = Vertex;
            town.AddToPlayer(Player);
            town.AddToBoard(game.Board);
            var matchingHexWithPort = game.Board.HexesByLocation.Values
                .Where(h => h.Port != null)
                .FirstOrDefault(h => h.Port.Edge.Vertices.Contains(Vertex));
            if (matchingHexWithPort != null)
            {
                Player.Ports.Add(matchingHexWithPort.Port);
            }

            base.Perform(game);
        }
    }
}
