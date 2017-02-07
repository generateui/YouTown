using System.Linq;

namespace YouTown.Validation
{
    public class CanBuildTownAt : ValidatorBase<Point, IBoard>
    {
        // In the future, this check should probably be made more specific e.g. to towns and cities
        public override IValidationResult Validate(Point point, IBoard board, string text = null)
        {
            bool pointIsTaken = board.PiecesByPoint.ContainsKey(point);
            bool neighboringPointsAreTaken = point.Neighbors
                .ToList()
                .Any(p => board.PiecesByPoint.ContainsKey(p));
            if (pointIsTaken)
            {
                return new Invalid($"point {point} is already taken");
            }
            if (neighboringPointsAreTaken)
            {
                return new Invalid($"neigbours of {point} are already taken");
            }
            return Validator.Valid;
        }
    }
}
