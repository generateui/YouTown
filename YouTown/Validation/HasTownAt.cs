namespace YouTown.Validation
{
    public class HasTownAt : ValidatorBase<IPlayer, Point>
    {
        public override IValidationResult Validate(IPlayer player, Point point, string text = null)
        {
            if (player.PointPieces.ContainsKey(point))
            {
                // TODO: have nicer description of location e.g. "3ore, 9wheat, 6clay"
                return new Invalid($"player {player.User.Name} does not have a town at {point}");
            }
            return Validator.Valid;
        }
    }
}