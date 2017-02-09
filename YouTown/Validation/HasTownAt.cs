namespace YouTown.Validation
{
    public class HasTownAt : ValidatorBase<IPlayer, Vertex>
    {
        public override IValidationResult Validate(IPlayer player, Vertex vertex, string text = null)
        {
            if (player.VertexPieces.ContainsKey(vertex))
            {
                // TODO: have nicer description of location e.g. "3ore, 9wheat, 6clay"
                return new Invalid($"player {player.User.Name} does not have a town at {vertex}");
            }
            return Validator.Valid;
        }
    }
}