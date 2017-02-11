using System;
using System.Linq;
using YouTown.Validation;

namespace YouTown.GameAction
{
    public class RobPlayer : GameActionBase
    {
        public static ActionType RobPlayerType = new ActionType("RobPlayer");

        public RobPlayer(IPlayer player) : base(player) { }
        public RobPlayer(int id, IPlayer player, IPlayer opponent, IResource resource) : base(id, player)
        {
            Opponent = opponent;
            Resource = resource;
        }
        public RobPlayer(RobPlayerData data, IRepository repo) : base(data, repo)
        {
            Opponent = repo.GetOrNull<IPlayer>(data.OpponentId);
            Resource = data.Resource?.FromData();
        }

        public override ActionType ActionType => RobPlayerType;
        public IPlayer Opponent { get; }
        public IResource Resource { get; private set; }

        public override bool IsAllowedInTurnPhase(ITurnPhase tp) => tp.IsBeforeDiceRoll || tp.IsDiceRoll || tp.IsBuilding;
        public override bool IsAllowedInGamePhase(IGamePhase gp) => gp.IsTurns;

        public override GameActionData ToData() =>
            base.ToData(new RobPlayerData
            {
                GameActionType = GameActionTypeData.RobPlayer,
                OpponentId = Opponent?.Id,
                Resource = Resource.ToData()
            });

        public override IValidationResult Validate(IGame game)
        {
            // TODO: conditionally validate
            throw new NotImplementedException();
        }

        public override void PerformAtServer(IServerGame serverGame)
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

        public override void Perform(IGame game)
        {
            if (Resource != null)
            {
                var resourceList = new ResourceList(Resource);
                // TODO: obscurable
                Player.GainResourcesFrom(Opponent.Hand, resourceList, null);
            }

            base.Perform(game);
        }
    }
}
