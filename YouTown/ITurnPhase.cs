namespace YouTown
{
    public interface ITurnPhase : IGameItem
    {
        bool IsBeforeDiceRoll { get; }
        bool IsDiceRoll { get; }
        bool IsTrading { get; }
        bool IsBuilding { get; }
    }

    public abstract class TurnPhaseBase
    {
        protected TurnPhaseBase(int id)
        {
            Id = id;
        }
        public int Id { get;  }
        public virtual bool IsBeforeDiceRoll => false;
        public virtual bool IsDiceRoll => false;
        public virtual bool IsTrading => false;
        public virtual bool IsBuilding => false;
    }

    public class BeforeDiceRoll : TurnPhaseBase, ITurnPhase
    {
        public BeforeDiceRoll(int id) : base(id) { }
        public override bool IsBeforeDiceRoll => true;
    }
    public class RollDicePhase : TurnPhaseBase, ITurnPhase
    {
        public RollDicePhase(int id) : base(id) { }
        public override bool IsDiceRoll => true;
    }
    public class Trading : TurnPhaseBase, ITurnPhase
    {
        public Trading(int id) : base(id) { }
        public override bool IsTrading => true;
    }
    public class Building : TurnPhaseBase, ITurnPhase
    {
        public Building(int id) : base(id) { }
        public override bool IsBuilding => true;
    }
}
