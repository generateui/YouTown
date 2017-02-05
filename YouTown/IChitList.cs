using System.Collections.Generic;

namespace YouTown
{
    public interface IChitList
    {
        IList<IChit> Chits { get; }
    }

    public class ChitList : IChitList
    {
        public ChitList(IList<IChit> chits)
        {
            Chits = chits;
        }

        public IList<IChit> Chits { get; }
    }
}