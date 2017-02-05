using System.Collections.Generic;

namespace YouTown
{
    public interface IHexList
    {
        IList<IHex> Hexes { get; }
    }

    public class HexList : IHexList
    {
        public HexList(IList<IHex> hexes)
        {
            Hexes = hexes;
        }

        public IList<IHex> Hexes { get; }
    }
}
