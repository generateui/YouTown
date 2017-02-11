namespace YouTown.Validation
{
    public class CanPlaceRobberAt : ValidatorBase<IBoardForPlay, Location>
    {
        public override IValidationResult Validate(IBoardForPlay board, Location location, string text = null)
        {
            var hex = board.HexesByLocation[location];
            if (!hex.CanHaveRobber)
            {
                return new Invalid($"hex {hex} at location {location} does not support placing a robber on it");
            }
            return Validator.Valid;
        }
    }
}