using System.Linq;
using YouTown.Validator;

namespace YouTown.GameAction
{
    public class BuildCity : IGameAction
    {
        public BuildCity(int id, IPlayer player, Point point)
        {
            Id = id;
            Player = player;
            Point = point;
        }

        public int Id { get; }
        public IPlayer Player { get; }
        public ITurnPhase TurnPhase { get; private set; }
        public IGamePhase GamePhase { get; private set; }
        public ITurn Turn { get; private set; }
        public Point Point { get; }
        public bool IsAllowedInOpponentTurn => false;

        public bool IsAllowedInTurnPhase(ITurnPhase tp) => tp.IsBuilding;
        public bool IsAllowedInGamePhase(IGamePhase gp) => gp.IsInitialPlacement || gp.IsTurns;

        public IValidationResult Validate(IGame game) =>
            new ValidateAll()
                .WithObject<NotNull>(Point)
                .With<HasTownAt, IPlayer, Point>(Player, Point)
                .With<HasCityInStock, IPlayer>(Player)
//                .With<CanPayPiece, IPlayer, IPiece>(Player, City)
                .Validate();

        public void PerformAtServer(IServerGame serverGame)
        {
        }

        public void Perform(IGame game)
        {
            Town town = Player.Towns[Point];
            town.RemoveFromPlayer(Player);
            town.RemoveFromBoard(game.Board);

            City cityFromStock = Player.Stock[City.CityType].Last() as City;
            var city = new City(Player, cityFromStock.Id, Point);
            city.AddToPlayer(Player);
            city.AddToBoard(game.Board);
            cityFromStock.RemoveFromPlayer(Player);

            TurnPhase = game.PlayTurns.TurnPhase;
            GamePhase = game.GamePhase;
            Turn = game.PlayTurns.Turn;
        }

    }
}
