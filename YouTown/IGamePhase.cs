using System.Collections.Generic;
using System.Linq;

namespace YouTown
{
    public interface IGamePhase
    {
        void Start(IGame game);
        void End(IGame game);
//        void RollDice()

        void BuildTown(IGame game, IPlayer player, Town town);
        void BuildRoad(IGame game, IPlayer player, Road road);
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

        public virtual void BuildTown(IGame game, IPlayer player, Town town)
        {
        }

        public virtual void BuildRoad(IGame game, IPlayer player, Road road)
        {
        }
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
        private List<PlaceTurn> _turns = new List<PlaceTurn>();
        private IPlayer _playerOnTurn;
        private int _roadsBuilt;

        public void Start(IGame game)
        {
            _playerOnTurn = game.Players.First();
            _playerOnTurn.IsOnTurn = true;
        }

        public void End(IGame game)
        {
        }

        public override void BuildRoad(IGame game, IPlayer player, Road road)
        {
            _roadsBuilt++;
            bool to = _roadsBuilt < game.Players.Count;
            bool halfway = _roadsBuilt == game.Players.Count;
            bool back = _roadsBuilt > game.Players.Count;
            _playerOnTurn.IsOnTurn = false;
            if (to)
            {
                var index = game.Players.IndexOf(_playerOnTurn) + 1;
                _playerOnTurn = game.Players[index];
            } else if (halfway)
            {
                // nothing, player stays "on turn"
            } else if (back && _playerOnTurn.Equals(game.Players.First()))
            {
                // nothing, player stays "on turn"
            }
            else
            {
                var index = game.Players.IndexOf(_playerOnTurn) - 1;
                _playerOnTurn = game.Players[index];
            }
            _playerOnTurn.IsOnTurn = true;
            int expectedAmountRoadsBuilt = game.Players.Count * 2;
            if (_roadsBuilt == expectedAmountRoadsBuilt)
            {
                game.MoveToNextPhase();
            }
        }

        public override void BuildTown(IGame game, IPlayer player, Town town)
        {
            if (player.Towns.Count != 1)
            {
                return;
            }
            var resourcesGained = new List<IResource>();
            foreach (Location location in town.Point.Locations)
            {
                IHex hex = game.Board.HexesByLocation[location];
                IResource resource = hex.Produce();
                if (resource != null)
                {
                    resourcesGained.Add(resource);
                }
            }
            player.GainResourcesFrom(game.Bank.Resources, new ResourceList(resourcesGained), null);
            // TODO: indicate to all players what resources this player gained
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
            Turns = new List<IPlayTurnsTurn>();
            TurnPhase = _beforeDiceRoll;
        }
        public override bool IsTurns => true;
        public IList<IPlayTurnsTurn> Turns { get; }
        public IPlayTurnsTurn Turn { get; }
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
