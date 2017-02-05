using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouTown
{
    public interface IGame
    {
        IBank Bank { get; }
        IList<IPlayer> Players { get; }
        IIdentifier Identifier { get; }
    }
}
