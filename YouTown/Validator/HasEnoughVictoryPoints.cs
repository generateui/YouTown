namespace YouTown.Validator
{
    public class HasEnoughVictoryPoints : ValidatorBase<IPlayer, IGame>
    {
        public override IValidationResult Validate(IPlayer player, IGame game, string text = null)
        {
            int points = player.TotalVictoryPoints;
            int needed = game.Options.VictoryPointsToWin;
            if (points < needed)
            {
                return new Invalid($"player {player.User.Name} has {points} but needs at least {needed} to claim victory");
            }
            return Validator.Valid;
        }
    }
}
