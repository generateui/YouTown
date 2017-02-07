using System.Linq;

namespace YouTown.Validator
{
    public class HasRoadToPoint : ValidatorBase<Point, IPlayer>
    {
        public override IValidationResult Validate(Point point, IPlayer player, string text = null)
        {
            bool hasRoadTo = player.Roads.Keys
                .SelectMany(edge => edge.Points)
                .Any(p => p.Equals(point));
            if (!hasRoadTo)
            {
                return new Invalid($"player {player.User.Name} does not have a road leading to {point}");
            }
            return Validator.Valid;
        }
    }
}
