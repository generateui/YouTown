using System.Linq;

namespace YouTown.Validation
{
    public class HasRoadInStock : ValidatorBase<IPlayer>
    {
        public override IValidationResult Validate(IPlayer player, string text = null)
        {
            if (!player.Stock[Road.RoadType].Any())
            {
                return new Invalid($"player {player.User.Name} does not have a road in stock");
            }
            return Validator.Valid;
        }
    }
}
