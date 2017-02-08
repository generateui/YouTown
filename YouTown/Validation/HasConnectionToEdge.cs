using System.Linq;

namespace YouTown.Validation
{
    public class HasConnectionToEdge : ValidatorBase<Edge, IPlayer>
    {
        public override IValidationResult Validate(Edge edge, IPlayer player, string text = null)
        {
            var neighbors = edge.Neighbors;
            var hasRoad = player.Roads.Keys.Any(e => neighbors.Contains(e));
            var hasTownOrCity = player.Towns.Keys
                .Concat(player.Cities.Keys)
                .Any(p => edge.Point1.Equals(p) || edge.Point2.Equals(p));
            if (!hasRoad && !hasTownOrCity)
            {
                return new Invalid($"player {player.User.Name} does not have a road leading to edge {edge} or a town/city on the edges' points");
            }
            return Validator.Valid;
        }
    }
}
