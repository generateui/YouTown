namespace YouTown
{
    public interface ITurnPhase
    {
        bool IsBeforeDiceRoll { get; }
        bool IsDiceRoll { get; }
        bool IsTrading { get; }
        bool IsBuilding { get; }
    }

    public abstract class TurnPhaseBase
    {
        public virtual bool IsBeforeDiceRoll => false;
        public virtual bool IsDiceRoll => false;
        public virtual bool IsTrading => false;
        public virtual bool IsBuilding => false;
    }

    public class BeforeDiceRoll : TurnPhaseBase, ITurnPhase
    {
        public override bool IsBeforeDiceRoll => true;
    }
    public class RollDicePhase : TurnPhaseBase, ITurnPhase
    {
        public override bool IsDiceRoll => true;
    }
    public class Trading : TurnPhaseBase, ITurnPhase
    {
        public override bool IsTrading => true;
    }
    public class Building : TurnPhaseBase, ITurnPhase
    {
        public override bool IsBuilding => true;
    }
}
