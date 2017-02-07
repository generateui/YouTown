namespace YouTown.Validation
{
    public class HasHexAt : ValidatorBase<IBoard, Location>
    {
        public override IValidationResult Validate(IBoard board, Location location, string text = null)
        {
            if (!board.HexesByLocation.ContainsKey(location))
            {
                return new Invalid($"board does not have a hex at location {location}");
            }
            return Validator.Valid;
        }
    }
}