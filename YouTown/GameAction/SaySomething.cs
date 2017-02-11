using System;
using YouTown.Validation;

namespace YouTown.GameAction
{
    public class SaySomething : GameActionBase, IGameAction
    {
        public static ActionType SaySomethingType = new ActionType("SaySomething");

        public SaySomething(int id, IPlayer player, string what) : base(id, player)
        {
            What = what;
        }
        public SaySomething(SaySomethingData data, IRepository repo) : base(data, repo)
        {
            What = data.Text;
        }

        public override ActionType ActionType => SaySomethingType;
        public string What { get; }
        public override bool IsAllowedInOpponentTurn => true;

        public override bool IsAllowedInTurnPhase(ITurnPhase turnPhase) => true;
        public override bool IsAllowedInGamePhase(IGamePhase gamePhase) => true;

        public override GameActionData ToData() =>
            base.ToData(new SaySomethingData
            {
                GameActionType = GameActionTypeData.SaySomething,
                Text = What,
            });

        public override IValidationResult Validate(IGame game) => Validator.Valid;

        public override void Perform(IGame game)
        {
            var chat = new Chat(Player, Player.User, What, DateTime.Now);
            game.Chats.Add(chat);

            base.Perform(game);
        }
    }
}
