using System;

namespace YouTown
{
    public class LargestArmy : IPiece, IVictoryPoint
    {
        public static readonly PieceType LargestArmyType = new PieceType("largestarmy");
        public LargestArmy(int id = Identifier.DontCare)
        {
            Id = id;
        }

        public int Id { get; }
        public PieceType PieceType => LargestArmyType;
        public IPlayer Player { get; set; }
        public bool AffectsRoad => false;
        public IResourceList Cost => ResourceList.Empty;
        public int VictoryPoints => 2;

        public void AddToPlayer(IPlayer player)
        {
            player.Pieces.Add(this);
            player.VictoryPoints.Add(this);
        }

        public void RemoveFromPlayer(IPlayer player)
        {
            player.Pieces.Remove(this);
            player.VictoryPoints.Remove(this);
        }
    }
}
