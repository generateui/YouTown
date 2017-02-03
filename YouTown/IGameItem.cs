namespace YouTown
{
    /// <summary>
    /// Any thing needing identification and replication on the client side
    /// </summary>
    /// When implementing a client/server model, we want to have some guarantees that they're 
    /// both talking about the same thing in-game.
    /// Furthermore, this concept enables fast lookup on a repository.
    public interface IGameItem
    {
        /// <summary>
        /// Within the scope of a game an int is more then sufficient to identify an item
        /// </summary>
        int Id { get; }
    }
}
