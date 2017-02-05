namespace YouTown
{
    /// <summary>
    /// Some actions need obscuring such as dealing out resources at the 
    /// client. This goes for [GameAction]s like [RollDice] * but also 
    /// [DevelopmentCard]s like[Monopoly].
    /// </summary>
    public interface IObscurable
    {
        bool IsAtServer { get; }
        IPlayer PlayerAtClient { get; }
    }
}
