using System.Linq;
using YouTown.Validation;

namespace YouTown.GameAction
{
    public class BuildCity : IGameAction
    {
        public static ActionType BuildCityType = new ActionType("BuildCity");
        public BuildCity(int id, IPlayer player, Vertex vertex)
        {
            Id = id;
            Player = player;
            Vertex = vertex;
        }

        public int Id { get; }
        public ActionType ActionType => BuildCityType;
        public IPlayer Player { get; }
        public ITurnPhase TurnPhase { get; private set; }
        public IGamePhase GamePhase { get; private set; }
        public ITurn Turn { get; private set; }
        public Vertex Vertex { get; }
        public bool IsAllowedInOpponentTurn => false;

        public bool IsAllowedInTurnPhase(ITurnPhase tp) => tp.IsBuilding;
        public bool IsAllowedInGamePhase(IGamePhase gp) => gp.IsInitialPlacement || gp.IsTurns;

        public IValidationResult Validate(IGame game) =>
            new ValidateAll()
                .WithObject<NotNull>(Vertex)
                .With<HasTownAt, IPlayer, Vertex>(Player, Vertex)
                .With<HasCityInStock, IPlayer>(Player)
//                .With<CanPayPiece, IPlayer, IPiece>(Player, City)
                .Validate();

        public void PerformAtServer(IServerGame serverGame)
        {
        }

        public void Perform(IGame game)
        {
            Town town = Player.Towns[Vertex];
            town.RemoveFromPlayer(Player);
            town.RemoveFromBoard(game.Board);

            City cityFromStock = Player.Stock[City.CityType].Last() as City;
            var city = new City(Player, cityFromStock.Id, Vertex);
            city.AddToPlayer(Player);
            city.AddToBoard(game.Board);
            cityFromStock.RemoveFromPlayer(Player);

            TurnPhase = game.PlayTurns.TurnPhase;
            GamePhase = game.GamePhase;
            Turn = game.PlayTurns.Turn;
        }

    }
}
