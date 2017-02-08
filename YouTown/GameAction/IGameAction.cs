using YouTown.Validation;

namespace YouTown.GameAction
{
    public sealed class ActionType
    {
        private readonly string _actionType;

        public ActionType(string actionType)
        {
            _actionType = actionType;
        }

        private bool Equals(ActionType other)
        {
            return string.Equals(_actionType, other._actionType);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is ActionType && Equals((ActionType) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return _actionType?.GetHashCode() ?? 0;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"ActionType: {_actionType}";
        }
    }

    public interface IGameAction : IGameItem
    {
        ActionType ActionType { get; }
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
