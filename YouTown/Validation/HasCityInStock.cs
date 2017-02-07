using System.Linq;

namespace YouTown.Validation
{
    public class HasCityInStock : ValidatorBase<IPlayer>
    {
        public override IValidationResult Validate(IPlayer player, string text = null)
        {
            if (!player.Stock[City.CityType].Any())
            {
                return new Invalid($"player {player.User.Name} does not have a city in stock");
            }
            return Validator.Valid;
        }
    }
}