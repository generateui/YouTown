using System;
using System.Linq;
using YouTown.Validation;

namespace YouTown.GameAction
{
    public class RobPlayer : IGameAction
    {
        public RobPlayer(IPlayer player)
        {
            Player = player;
        }
        public RobPlayer(int id, IPlayer player, IPlayer opponent, IResource resource)
        {
            Id = id;
            Player = player;
            Opponent = opponent;
            Resource = resource;
        }

        public int Id { get; }
        public IPlayer Player { get; }
        public IPlayer Opponent { get; }
        public IResource Resource { get; private set; }
        public ITurnPhase TurnPhase { get; private set; }
        public IGamePhase GamePhase { get; private set; }
        public ITurn Turn { get; private set; }
        public bool IsAllowedInOpponentTurn => false;

        public bool IsAllowedInTurnPhase(ITurnPhase tp) => tp.IsBeforeDiceRoll || tp.IsDiceRoll || tp.IsBuilding;
        public bool IsAllowedInGamePhase(IGamePhase gp) => gp.IsTurns;

        public IValidationResult Validate(IGame game)
        {
            // TODO: conditionally validate
            throw new NotImplementedException();
        }

        public void PerformAtServer(IServerGame serverGame)
        {
            if (Opponent == null)
            {
                return;
            }
            if (!Opponent.Hand.Any())
            {
                return;
            }
            var random = serverGame.Random;
            Resource = Opponent.Hand.PickRandom(random);
        }

        public void Perform(IGame game)
        {
            if (Resource != null)
            {
                var resourceList = new ResourceList(Resource);
                // TODO: obscurable
                Player.GainResourcesFrom(Opponent.Hand, resourceList, null);
            }

            TurnPhase = game.PlayTurns.TurnPhase;
            GamePhase = game.GamePhase;
            Turn = game.PlayTurns.Turn;
        }
    }
}
