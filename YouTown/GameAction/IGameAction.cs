using YouTown.Validation;

namespace YouTown.GameAction
{
    public interface IGameAction : IGameItem
    {
        IPlayer Player { get; }
        ITurnPhase TurnPhase { get; }
        IGamePhase GamePhase { get; }
        ITurn Turn { get; }
        bool IsAllowedInOpponentTurn { get; }
        bool IsAllowedInTurnPhase(ITurnPhase turnPhase);
        bool IsAllowedInGamePhase(IGamePhase gamePhase);
        IValidationResult Validate(IGame game);
        void PerformAtServer(IServerGame serverGame);
        void Perform(IGame game);
    }
}
