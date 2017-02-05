using System.Collections.Generic;

namespace YouTown
{
    public interface IGamePhase
    {
        void Start(IGame game);
        void End(IGame game);
//        void RollDice()
//buildtown
//buildroad
        bool IsSetup { get; }
        bool IsDetermineFirstPlayer { get; }
        bool IsInitialPlacement { get; }
        bool IsTurns { get; }
        bool IsEnd { get; }
    }

    public abstract class GamePhaseBase
    {
        public virtual bool IsSetup => false;
        public virtual bool IsDetermineFirstPlayer => false;
        public virtual bool IsInitialPlacement => false;
        public virtual bool IsTurns => false;
        public virtual bool IsEnd => false;
    }

    /// <summary>
    /// A mechanism must determine the first player and the order of players.
    /// </summary>
    /// Many mechanisms are used to do this: 
    /// - let the server randomly decide
    /// - have players roll a dice until a highroller turns up
    /// - setting in the game: first come, first serve (gives host deterministic 
    ///   first spot)
    /// For initial simplicity sake of implementation, we let the server decide for now
    /// as default implementation.
    public class DetermineFirstPlayer : GamePhaseBase, IGamePhase
    {
        public override bool IsDetermineFirstPlayer => true;

        public void Start(IGame game)
        {
        }

        public void End(IGame game)
        {
        }
    }

    /// <summary>
    /// Housekeeping phase for animation purposes
    /// </summary>
    /// Imagine a start state where the board is not setup and players
    /// do not yet have any pieces. It would be nice for players to
    /// see the board transform from a design state to a playable state.
    /// Further, having the pieces appear animated at each player and the 
    /// bank clarify what's in the game.
    public class SetupGamePhase : GamePhaseBase, IGamePhase
    {
        public override bool IsSetup => true;
        public void Start(IGame game)
        {
        }

        public void End(IGame game)
        {
        }
    }

    /// <summary>
    /// Player place each 2 towns and 2 roads to start with
    /// </summary>
    /// There is an alternative setup where player get one town, one city and three
    /// roads. The order then is town+road, (other players), city+road+road.
    public class PlaceInitialPieces : GamePhaseBase, IGamePhase
    {
        public override bool IsInitialPlacement => true;

        public void Start(IGame game)
        {
        }

        public void End(IGame game)
        {
        }
    }

    /// <summary>
    /// Longest phase of all in the game where players switch turns
    /// </summary>
    public class PlayTurns : GamePhaseBase, IGamePhase
    {
        private readonly BeforeDiceRoll _beforeDiceRoll = new BeforeDiceRoll();
        private readonly DiceRoll _diceRoll = new DiceRoll();
        private readonly Trading _trading = new Trading();
        private readonly Building _building = new Building();

        public PlayTurns()
        {
            Turns = new List<ITurn>();
            TurnPhase = _beforeDiceRoll;
        }
        public override bool IsTurns => true;
        public IList<ITurn> Turns { get; }
        public ITurn Turn { get; }
        public ITurnPhase TurnPhase { get; private set; }

        public void Start(IGame game)
        {
        }

        public void End(IGame game)
        {
        }

        public void SetToBeforeDiceRoll()
        {
            if (TurnPhase != _beforeDiceRoll)
            {
                TurnPhase = _beforeDiceRoll;
            }
        }

        public void SetToDiceRoll()
        {
            if (TurnPhase != _diceRoll)
            {
                TurnPhase = _diceRoll;
            }
        }

        public void SetToTrading()
        {
            if (TurnPhase != _trading)
            {
                TurnPhase = _trading;
            }
        }

        public void SetToBuilding()
        {
            if (TurnPhase != _building)
            {
                TurnPhase = _building;
            }
        }
    }

    public class EndOfGame : GamePhaseBase, IGamePhase
    {
        public override bool IsEnd => true;

        public void Start(IGame game)
        {
        }

        public void End(IGame game)
        {
        }
    }
}
