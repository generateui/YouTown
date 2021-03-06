﻿using System;

namespace YouTown
{
    public class LargestArmy : IPiece, IVictoryPoint
    {
        public static readonly PieceType LargestArmyType = new PieceType("largestarmy");

        public LargestArmy(int id = Identifier.DontCare)
        {
            Id = id;
        }
        public LargestArmy(LargestArmyData data, IRepository repo)
        {
            Id = data.Id;
            Player = data.PlayerId.HasValue ? repo.Get<IPlayer>(data.PlayerId.Value) : null;
        }

        public int Id { get; }
        public PieceType PieceType => LargestArmyType;
        public IPlayer Player { get; set; }
        public bool AffectsRoad => false;
        public IResourceList Cost => ResourceList.Empty;
        public int VictoryPoints => 2;

        public LargestArmyData ToData() =>
            new LargestArmyData
            {
                Id = Id,
                PlayerId = Player?.Id
            };

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

        public void AddToBoard(IBoardForPlay board)
        {
            throw new NotImplementedException();
        }

        public void RemoveFromBoard(IBoardForPlay board)
        {
            throw new NotImplementedException();
        }
    }
}
