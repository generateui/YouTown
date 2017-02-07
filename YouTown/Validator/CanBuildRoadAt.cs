namespace YouTown.Validator
{
    public class CanBuildRoadAt : ValidatorBase<Edge, IBoard>
    {
        public override IValidationResult Validate(Edge edge, IBoard board, string text = null)
        {
            if (board.PiecesByEdge.ContainsKey(edge))
            {
                return new Invalid($"there is already a piece at edge {edge}");
            }
            return Validator.Valid;
        }
    }
}
