using System.Collections.Generic;
using System.Linq;
using YouTown.GameAction;

namespace YouTown
{
    public interface IGamePhase
    {
        bool IsSetup { get; }
        bool IsDetermineFirstPlayer { get; }
        bool IsInitialPlacement { get; }
        bool IsTurns { get; }
        bool IsEnd { get; }

        void Start(IGame game);
        void End(IGame game);

        Production RollDice(IGame game, DiceRoll diceRoll, IPlayer player);
        void BuildTown(IGame game, IPlayer player, Town town);
        void BuildRoad(IGame game, IPlayer player, Road road);
    }

    public abstract class GamePhaseBase
    {
        public virtual bool IsSetup => false;
        public virtual bool IsDetermineFirstPlayer => false;
        public virtual bool IsInitialPlacement => false;
        public virtual bool IsTurns => false;
        public virtual bool IsEnd => false;

        public virtual void BuildTown(IGame game, IPlayer player, Town town) { }
        public virtual void BuildRoad(IGame game, IPlayer player, Road road) { }
        public virtual Production RollDice(IGame game, DiceRoll diceRoll, IPlayer player)
        {
            return null;
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
        private readonly RollDicePhase _diceRoll = new RollDicePhase();
        private readonly Trading _trading = new Trading();
        private readonly Building _building = new Building();

        public PlayTurns()
        {
            Turns = new List<IPlayTurnsTurn>();
            TurnPhase = _beforeDiceRoll;
        }
        public override bool IsTurns => true;
        public IList<IPlayTurnsTurn> Turns { get; }
        public IPlayTurnsTurn Turn { get; private set; }
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

        public void MoveToNextTurn(IGame game)
        {
            var id = game.Identifier.NewId();
            var number = Turn.Number + 1;
            var player = game.Players.Next(Turn.Player);
            Turn = new PlayTurnsTurn(id, number, player);
        }

        public override Production RollDice(IGame game, DiceRoll diceRoll, IPlayer player)
        {
            SetToDiceRoll();
            int roll = diceRoll.Sum;
            if (roll == 7)
            {
                var looseCards = game.Players
                    .Where(p => p.Hand.Count > 7)
                    .Select(p => new LooseCards(p));
                game.Queue.EnqueueUnordered(looseCards);
                game.Queue.EnqueueSingle(new MoveRobber(player), optional: false);
                game.Queue.EnqueueSingle(new RobPlayer(player), optional: true);
                return null;
            }
            // Produce resources:
            // 1. determine productions
            // 2. determine production per player
            // 3. determine shortages
            // 4. determine resources to distribute
            // 5. distribute resources
            var hexes = game.Board.HexesByLocation.Values
                .Where(h => h.Chit != null)
                .Where(h => h.Chit.Number == roll)
                .Where(h => !h.Location.Equals(game.Board.Robber.Location));

            var productions = new List<Produce>();
            foreach (var hex in hexes)
            {
                var produced = game.Board.Producers
                    .Where(p => p.IsAt(hex.Location))
                    .Select(p => new Produce(p, hex, p.Produce(hex)));
                productions.AddRange(produced);
            }
            var producedResources = new ResourceList(productions.SelectMany(p => p.Resources));
            var resourcesByPlayer = new Dictionary<IPlayer, IResourceList>();
            var players = productions.Select(p => p.Producer.Player).Distinct();
            foreach (var player1 in players)
            {
                var resources = productions
                    .Where(p => p.Producer.Player.Equals(player1))
                    .SelectMany(p => p.Resources);
                var resourceList = new ResourceList(resources);
                resourcesByPlayer[player1] = resourceList;
            }
            var toDistribute = new Dictionary<IPlayer, IResourceList>(resourcesByPlayer);
            var shortages = new List<ProductionShortage>();
            if (game.Bank.Resources.HasAtLeast(producedResources))
            {
                return new Production(toDistribute, productions, shortages);
            }

            // divide shortage among players
            // players are assigned shortages starting at the player last on turn.
            // example: p0, p1, p2, [p3], p4 (p3 is on turn)
            // p2 gets first shortage, then p1, p0, p4, p3.
            // note: alternative ways of dealing with this situation exist:
            // - don't contrain bank on resources (there's always enough)
            // - randomly assign shortage (but balanced)
            // - start by player having most victory points (incentivizes playing VictoryPoint devcards)
            var bank = game.Bank.Resources;
            foreach (var resourceType in producedResources.ResourceTypes)
            {
                var resourceOfType = producedResources.OfType(resourceType);
                var bankCount = bank.OfType(resourceType).Count;
                if (bankCount == resourceOfType.Count)
                {
                    continue;
                }
                var players1 = productions
                    .Where(p => p.Resources.HasType(resourceType))
                    .Select(p => p.Producer.Player)
                    .Distinct()
                    .ToList();
                var shortage = resourceOfType.Count - bankCount;
                var shortagePerPlayer = shortage/players1.Count;
                var shortageRemainder = shortage%players1.Count;
                var shortageByPlayer = new Dictionary<IPlayer, int>();
                int indexOfCurrentPlayer = game.Players.IndexOf(player);
                while (shortageRemainder > 0)
                {
                    int index = indexOfCurrentPlayer == 0 ? game.Players.Count - 1 : indexOfCurrentPlayer - 1;
                    var player1 = game.Players[index];
                    if (players1.Contains(player1))
                    {
                        shortageByPlayer[player1] = shortagePerPlayer + 1;
                        shortageRemainder--;
                    }
                }
                foreach (var pair in shortageByPlayer)
                {
                    var player1 = pair.Key;
                    var amountShort = pair.Value;
                    shortages.Add(new ProductionShortage(player1, resourceType, amountShort));
                }
            }
            foreach (var shortage in shortages)
            {
                var resources = toDistribute[shortage.Player];
                var removed = resources.ToList();
                for (int i = 0; i < shortage.AmountShort; i++)
                {
                    var toRemove = removed.Last();
                    removed.Remove(toRemove);
                }
                toDistribute[shortage.Player] = new ResourceList(removed);
            }

            // Actually distribute the resources
            foreach (KeyValuePair<IPlayer, IResourceList> pair in toDistribute)
            {
                var player1 = pair.Key;
                var resourcesToGain = pair.Value;
                player1.GainResourcesFrom(game.Bank.Resources, resourcesToGain, null);
            }

            return new Production(toDistribute, productions, shortages);
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
