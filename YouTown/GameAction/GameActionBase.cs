using System;
using YouTown.Validation;

namespace YouTown.GameAction
{
    public abstract class GameActionBase : IGameAction
    {
        protected GameActionBase(IPlayer player)
        {
            Player = player;
        }
        protected GameActionBase(int id, IPlayer player)
        {
            Id = id;
            Player = player;
        }
        protected GameActionBase(GameActionData data, IRepository repo)
        {
            Id = data.Id;
            Player = repo.Get<IPlayer>(data.PlayerId);
            TurnPhase = repo.GetOrNull<ITurnPhase>(data.TurnPhaseId);
            GamePhase = repo.GetOrNull<IGamePhase>(data.GamePhaseId);
            Turn = repo.GetOrNull<ITurn>(data.TurnId);
        }

        public int Id { get; }
        public virtual ActionType ActionType { get { throw new NotImplementedException(); } }
        public IPlayer Player { get; }
        public ITurnPhase TurnPhase { get; private set; }
        public IGamePhase GamePhase { get; private set; }
        public ITurn Turn { get; private set; }
        public virtual bool IsAllowedInOpponentTurn => false;

        public virtual GameActionData ToData(GameActionData specific)
        {
            specific.Id = Id;
            specific.PlayerId = Player.Id;
            specific.TurnPhaseId = TurnPhase.Id;
            specific.GamePhaseId = GamePhase.Id;
            specific.TurnId = Turn.Id;
            return specific;
        }

        public virtual bool IsAllowedInTurnPhase(ITurnPhase turnPhase)
        {
            throw new NotImplementedException();
        }

        public virtual bool IsAllowedInGamePhase(IGamePhase gamePhase)
        {
            throw new NotImplementedException();
        }

        public virtual IValidationResult Validate(IGame game)
        {
            throw new NotImplementedException();
        }

        protected ValidateAll BaseValidate(IGame game) =>
            new ValidateAll()
                .WithObject<NotNull>(Player);

        public virtual void PerformAtServer(IServerGame serverGame)
        {
        }

        public virtual void Perform(IGame game)
        {
            TurnPhase = game.PlayTurns.TurnPhase;
            GamePhase = game.GamePhase;
            Turn = game.PlayTurns.Turn;
            game.Repository.Add(this);
            game.Actions.Add(this);
        }

        public virtual GameActionData ToData()
        {
            throw new NotImplementedException();
        }
    }
}
