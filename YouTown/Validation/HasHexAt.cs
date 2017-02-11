namespace YouTown.Validation
{
    public class HasHexAt : ValidatorBase<IBoardForPlay, Location>
    {
        public override IValidationResult Validate(IBoardForPlay board, Location location, string text = null)
        {
            if (!board.HexesByLocation.ContainsKey(location))
            {
                return new Invalid($"board does not have a hex at location {location}");
            }
            return Validator.Valid;
        }
    }
}