﻿using System.Collections.Generic;
using System.Linq;
using Bond;
using YouTown.GameAction;

namespace YouTown
{
    /// <summary>
    /// Enables mandatory and non-mandatory expectations on game actions 
    /// </summary>
    /// Say a player performs a <see cref="Soldier"/> development card. In itself,
    /// this won't do much to the gamestate except setting the expectation that this
    /// player must move the robber and then may rob a player. So, the soldier impl
    /// simply enqueues a <see cref="MoveRobber"/> and then an optional 
    /// <see cref="RobPlayer"/> action.
    /// Similarly, the <see cref="PlaceInitialPieces"/> game phase enqueues actions
    /// at start of the phase where every player is expected to perform an action.
    /// Use cases:
    /// - <see cref="PlayDevelopmentCard"/> with <see cref="Soldier"/> enqueues a
    ///   <see cref="MoveRobber"/> then optional <see cref="RobPlayer"/>
    /// - <see cref="PlaceInitialPieces"/> enqueues in order:
    ///     - player 1 <see cref="BuildTown"/> then <see cref="BuildRoad"/>
    ///     - player 2 <see cref="BuildTown"/> then <see cref="BuildRoad"/>
    ///     - player 3 <see cref="BuildTown"/> then <see cref="BuildRoad"/>
    ///     - player 4 <see cref="BuildTown"/> then <see cref="BuildRoad"/>
    ///     - player 4 <see cref="BuildTown"/> then <see cref="BuildRoad"/>
    ///     - player 3 <see cref="BuildTown"/> then <see cref="BuildRoad"/>
    ///     - player 2 <see cref="BuildTown"/> then <see cref="BuildRoad"/>
    ///     - player 1 <see cref="BuildTown"/> then <see cref="BuildRoad"/>
    /// - <see cref="RollDice" /> when 7 rolls, all players must 
    ///   <see cref="LooseCards" />. This is unordered.
    /// - When <see cref="OfferTrade"/> all opponents should respond without specific 
    ///   order. However, when the timeout happens, this must be remvoed.
    /// 
    /// Notable exception: playing a <see cref="RoadBuilding"/>: this uses tokens
    /// to optionally build roads during the <see cref="Building"/> phase.
    public interface IActionQueue
    {
        /// <summary>
        /// True when given action satisfies the expectation of the queue
        /// </summary>
        /// In the example of the <see cref="Soldier"/>, a followup action of type
        /// <see cref="MoveRobber"/> would satisfy. Yet when the player plays a 
        /// <see cref="RobPlayer"/> first, it is not satisfactory according to the 
        /// state of the queue.
        bool Satisfies(IGameAction toPlay, bool mustBePresent = false);

        void EnqueueSingle(IGameAction action, bool optional = false);

        /// <summary>
        /// Enqueues given sequence of gameactions where order is unimportant.
        /// </summary>
        /// When a 7 is rolled, some players must <see cref="LooseCards"/>. Expected 
        /// order of these actions is unimportant, but it *is* important *all* actions
        /// are performed before continuing.
        void EnqueueUnordered(IEnumerable<IGameAction> unorderedActions);

        /// <summary>
        /// Enqueues given sequence of gameactions where order is important.
        /// </summary>
        /// The <see cref="PlaceInitialPieces"/> action sets the expectation to build 
        /// roads and towns in a specific order. Only after all actions are performed
        /// in given specific order, the game or gamephase can move on.
        void EnqueueOrdered(IList<IGameAction> orderedActions);

        void Dequeue(IGameAction action);
        List<QueuedItemGroupData> ToData();
    }

    public class ActionQueue : IActionQueue
    {
        private interface IItem
        {
            bool IsOptional { get; }
            bool Satisfies(IGameAction action);
            void Remove(Queue<IItem> queue, IGameAction toRemove);
        }

        private class Single : IItem
        {
            internal readonly IGameAction _gameAction;

            public Single(IGameAction gameAction, bool isOptional)
            {
                _gameAction = gameAction;
                IsOptional = isOptional;
            }

            public bool IsOptional { get; }

            public bool Satisfies(IGameAction action)
            {
                if (IsOptional)
                {
                    return true;
                }
                if (!action.ActionType.Equals(_gameAction.ActionType))
                {
                    return false;
                }
                if (!action.Player.Equals(_gameAction.Player))
                {
                    return false;
                }
                return true;
            }

            public void Remove(Queue<IItem> queue, IGameAction toRemove)
            {
                queue.Dequeue();
            }
        }

        private class Ordered : IItem
        {
            internal readonly List<Single> _actions;

            public Ordered(List<Single> actions)
            {
                _actions = actions;
            }

            public bool IsOptional => _actions.First().IsOptional;

            public bool Satisfies(IGameAction action)
            {
                return _actions.First().Satisfies(action);
            }

            public void Remove(Queue<IItem> queue, IGameAction toRemove)
            {
                _actions.RemoveAt(0);
                if (!_actions.Any())
                {
                    queue.Dequeue();
                }
            }
        }

        private class Unordered : IItem
        {
            internal readonly List<Single> _actions;

            public Unordered(IEnumerable<Single> actions)
            {
                _actions = actions.ToList();
            }

            public bool IsOptional => _actions.All(s => s.IsOptional);

            public bool Satisfies(IGameAction action)
            {
                return !_actions.Any() || _actions.Any(s => s.Satisfies(action));
            }

            public void Remove(Queue<IItem> queue, IGameAction toRemove)
            {
                Single itemToRemove = _actions.FirstOrDefault(s => s.Satisfies(toRemove));
                if (itemToRemove != null)
                {
                    _actions.Remove(itemToRemove);
                }
                if (!_actions.Any())
                {
                    queue.Dequeue();
                }
            }
        }

        private readonly Queue<IItem> _queue = new Queue<IItem>();

        public bool Satisfies(IGameAction toPlay, bool mustBePresent = false)
        {
            if (!_queue.Any())
            {
                return !mustBePresent;
            }
            return _queue.Peek().Satisfies(toPlay);
        }

        public void EnqueueSingle(IGameAction action, bool optional = false)
        {
            _queue.Enqueue(new Single(action, optional));
        }

        public void EnqueueUnordered(IEnumerable<IGameAction> unorderedActions)
        {
            if (!unorderedActions.Any())
            {
                return;
            }
            var unorderedItems = unorderedActions.Select(ga => new Single(ga, false));
            _queue.Enqueue(new Unordered(unorderedItems));
        }

        public void EnqueueOrdered(IList<IGameAction> orderedActions)
        {
            var orderedItems = orderedActions.Select(ga => new Single(ga, false)).ToList();
            _queue.Enqueue(new Ordered(orderedItems));
        }

        public void Dequeue(IGameAction action)
        {
            if (!_queue.Any())
            {
                return;
            }
            var first = _queue.Peek(); // item itself ensure removal
            first.Remove(_queue, action);
        }

        public List<QueuedItemGroupData> ToData()
        {
            var data = new List<QueuedItemGroupData>();
            foreach (IItem item in _queue)
            {
                var single = item as Single;
                if (single != null)
                {
                    data.Add(new QueuedItemGroupData
                    {
                        QueuedItemGroupType = QueuedItemGroupTypeData.Single,
                        Actions = new List<IBonded<GameActionData>>
                        {
                            new Bonded<GameActionData>(single._gameAction.ToData())
                        }
                    });
                }
                var ordered = item as Ordered;
                if (ordered != null)
                {
                    data.Add(new QueuedItemGroupData
                    {
                        QueuedItemGroupType = QueuedItemGroupTypeData.Ordered,
                        Actions = ordered._actions
                            .Select(s => new Bonded<GameActionData>(s._gameAction.ToData()))
                            .Cast<IBonded<GameActionData>>()
                            .ToList(),
                    });
                }
                var unordered = item as Unordered;
                if (unordered != null)
                {
                    data.Add(new QueuedItemGroupData
                    {
                        QueuedItemGroupType = QueuedItemGroupTypeData.Unordered,
                        Actions = unordered._actions
                            .Select(s => new Bonded<GameActionData>(s._gameAction.ToData()))
                            .Cast<IBonded<GameActionData>>()
                            .ToList(),
                    });
                }
            }
            return data;
        }
    }
}
