using System.Collections.Generic;
using YouTown.GameAction;

namespace YouTown
{
    public interface IActionQueue
    {
        void EnqueueSingle(IGameAction action, bool optional = false);
        void EnqueueUnordered(IEnumerable<IGameAction> unorderedActions);
        void EnqueueOrdered(IList<IGameAction> orderedActions);
    }
}
