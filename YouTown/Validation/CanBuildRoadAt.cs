namespace YouTown.Validation
{
    public class CanBuildRoadAt : ValidatorBase<Edge, IBoardForPlay>
    {
        public override IValidationResult Validate(Edge edge, IBoardForPlay board, string text = null)
        {
            if (board.PiecesByEdge.ContainsKey(edge))
            {
                return new Invalid($"there is already a piece at edge {edge}");
            }
            return Validator.Valid;
        }
    }
}
