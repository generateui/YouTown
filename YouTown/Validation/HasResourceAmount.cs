namespace YouTown.Validation
{
    public class HasResourceAmount : ValidatorBase<IPlayer, int>
    {
        public override IValidationResult Validate(IPlayer player, int amount, string text = null)
        {
            if (player.Hand.Count < amount)
            {
                return new Invalid($"player {player.User.Name} does not have {amount} resources");
            }
            return Validator.Valid;
        }
    }
}
