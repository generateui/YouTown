
namespace YouTown.Validator
{
    public class LoosesCorrectAmount : ValidatorBase<IPlayer, int>
    {
        public override IValidationResult Validate(IPlayer player, int amount, string text = null)
        {
            var amountToLoose = player.Hand.HalfCount();
            if (amountToLoose != amount)
            {
                return new Invalid($"player {player.User.Name} is expected to loose {amountToLoose} but wants to loose {amount}");
            }
            return Validator.Valid;
        }
    }
}