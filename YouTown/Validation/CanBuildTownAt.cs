using System.Linq;

namespace YouTown.Validation
{
    public class CanBuildTownAt : ValidatorBase<Vertex, IBoard>
    {
        // In the future, this check should probably be made more specific e.g. to towns and cities
        public override IValidationResult Validate(Vertex vertex, IBoard board, string text = null)
        {
            bool vertexIsTaken = board.PiecesByVertex.ContainsKey(vertex);
            bool neighboringVerticesAreTaken = vertex.Neighbors
                .ToList()
                .Any(p => board.PiecesByVertex.ContainsKey(p));
            if (vertexIsTaken)
            {
                return new Invalid($"vertex {vertex} is already taken");
            }
            if (neighboringVerticesAreTaken)
            {
                return new Invalid($"neigbours of {vertex} are already taken");
            }
            return Validator.Valid;
        }
    }
}
