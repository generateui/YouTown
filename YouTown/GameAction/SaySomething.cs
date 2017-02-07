using System;
using YouTown.Validation;

namespace YouTown.GameAction
{
    public class SaySomething : IGameAction
    {
        public SaySomething(int id, IPlayer player, string what)
        {
            Id = id;
            Player = player;
            What = what;
        }

        public int Id { get; }
        public IPlayer Player { get; }
        public ITurnPhase TurnPhase { get; private set; }
        public IGamePhase GamePhase { get; private set; }
        public ITurn Turn { get; private set; }
        public string What { get; }
        public bool IsAllowedInOpponentTurn => true;

        public bool IsAllowedInTurnPhase(ITurnPhase turnPhase) => true;
        public bool IsAllowedInGamePhase(IGamePhase gamePhase) => true;

        public IValidationResult Validate(IGame game) => Validator.Valid;

        public void PerformAtServer(IServerGame serverGame)
        {
        }

        public void Perform(IGame game)
        {
            var chat = new Chat(Player, Player.User, What, DateTime.Now);
            game.Chats.Add(chat);

            TurnPhase = game.PlayTurns.TurnPhase;
            GamePhase = game.GamePhase;
            Turn = game.PlayTurns.Turn;
        }
    }
}
