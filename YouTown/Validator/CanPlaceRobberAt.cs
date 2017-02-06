namespace YouTown.Validator
{
    public class CanPlaceRobberAt : ValidatorBase<IBoard, Location>
    {
        public override IValidationResult Validate(IBoard board, Location location, string text = null)
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