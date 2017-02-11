using System.Linq;
using YouTown.Validation;

namespace YouTown.GameAction
{
    public class BuildCity : GameActionBase
    {
        public static ActionType BuildCityType = new ActionType("BuildCity");
        public BuildCity(int id, IPlayer player, Vertex vertex) : base (id, player)
        {
            Vertex = vertex;
        }

        public BuildCity(BuildCityData data, IRepository repo) : base(data, repo)
        {
            Vertex = data.Vertex != null ? new Vertex(data.Vertex) : null;
        }

        public override ActionType ActionType => BuildCityType;
        public Vertex Vertex { get; }

        public override bool IsAllowedInTurnPhase(ITurnPhase tp) => tp.IsBuilding;
        public override bool IsAllowedInGamePhase(IGamePhase gp) => gp.IsInitialPlacement || gp.IsTurns;

        public override GameActionData ToData() =>
            base.ToData(new BuildCityData
            {
                GameActionType = GameActionTypeData.BuildCity,
                Vertex = Vertex?.ToData()
            });

        public override IValidationResult Validate(IGame game) =>
            BaseValidate(game)
                .WithObject<NotNull>(Vertex)
                .With<HasTownAt, IPlayer, Vertex>(Player, Vertex)
                .With<HasCityInStock, IPlayer>(Player)
//                .With<CanPayPiece, IPlayer, IPiece>(Player, City)
                .Validate();

        public override void Perform(IGame game)
        {
            Town town = Player.Towns[Vertex];
            town.RemoveFromPlayer(Player);
            town.RemoveFromBoard(game.Board);

            City cityFromStock = Player.Stock[City.CityType].Last() as City;
            var city = new City(Player, cityFromStock.Id, Vertex);
            city.AddToPlayer(Player);
            city.AddToBoard(game.Board);
            cityFromStock.RemoveFromPlayer(Player);

            base.Perform(game);
        }

    }
}
