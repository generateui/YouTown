using System.Linq;
using YouTown.Validation;

namespace YouTown.GameAction
{
    public class EndTurn : IGameAction
    {
        public static ActionType EndTurnType = new ActionType("EndTurn");
        public EndTurn(int id, IPlayer player)
        {
            Id = id;
            Player = player;
        }

        public int Id { get; }
        public ActionType ActionType => EndTurnType;
        public IPlayer Player { get; }
        public ITurnPhase TurnPhase { get; private set; }
        public IGamePhase GamePhase { get; private set; }
        public ITurn Turn { get; private set; }
        public bool IsAllowedInOpponentTurn => false;

        public bool IsAllowedInTurnPhase(ITurnPhase tp) => true;
        public bool IsAllowedInGamePhase(IGamePhase gp) => gp.IsTurns;

        public IValidationResult Validate(IGame game) => Validator.Valid;

        public void PerformAtServer(IServerGame serverGame)
        {
        }

        public void Perform(IGame game)
        {
            var tokens = Player.Pieces.OfType<RoadBuilding.Token>().ToList();
            foreach (RoadBuilding.Token token in tokens)
            {
                token.RemoveFromPlayer(Player);
            }
            game.PlayTurns.MoveToNextTurn(game);

            TurnPhase = game.PlayTurns.TurnPhase;
            GamePhase = game.GamePhase;
            Turn = game.PlayTurns.Turn;
        }
    }
}
