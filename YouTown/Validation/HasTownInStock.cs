using System.Linq;

namespace YouTown.Validation
{
    public class HasTownInStock : ValidatorBase<IPlayer>
    {
        public override IValidationResult Validate(IPlayer player, string text = null)
        {
            if (!player.Stock[Town.TownType].Any())
            {
                return new Invalid($"player {player.User.Name} does not have a town in stock");
            }
            return Validator.Valid;
        }
    }
}
