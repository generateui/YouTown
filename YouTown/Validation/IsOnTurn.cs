namespace YouTown.Validation
{
    public class IsOnTurn : ValidatorBase<IPlayer>
    {
        public override IValidationResult Validate(IPlayer player, string text = null)
        {
            if (!player.IsOnTurn)
            {
                return new Invalid($"it's not player {player.User.Name}'s turn");
            }
            return Validator.Valid;
        }
    }
}