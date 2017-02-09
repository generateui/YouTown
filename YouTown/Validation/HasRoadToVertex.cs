using System.Linq;

namespace YouTown.Validation
{
    public class HasRoadToVertex : ValidatorBase<Vertex, IPlayer>
    {
        public override IValidationResult Validate(Vertex vertex, IPlayer player, string text = null)
        {
            bool hasRoadTo = player.Roads.Keys
                .SelectMany(edge => edge.Vertices)
                .Any(p => p.Equals(vertex));
            if (!hasRoadTo)
            {
                return new Invalid($"player {player.User.Name} does not have a road leading to {vertex}");
            }
            return Validator.Valid;
        }
    }
}
