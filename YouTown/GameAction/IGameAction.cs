using YouTown.Validator;

namespace YouTown.GameAction
{
    public interface IGameAction : IGameItem
    {
        IPlayer Player { get; }
        ITurnPhase TurnPhase { get; }
        IGamePhase GamePhase { get; }
        ITurn Turn { get; }
        IValidationResult Validate(IGame game);
        void PerformAtServer(IServerGame serverGame);
        void Perform(IGame game);
        bool IsAllowedInTurnPhase(ITurnPhase turnPhase);
        bool IsAllowedInGamePhase(IGamePhase gamePhase);
        bool IsAllowedInOpponentTurn { get; }
    }
}
