namespace YouTown
{
    /// <summary>
    /// Token placeable on any <see cref="IHex"/> where the <see cref="IHex.CanHaveRobber"/>
    /// is true. 
    /// </summary>
    /// If there are any <see cref="Desert"/> at the board at start, the robber should be randomly placed
    /// on any of available <see cref="Desert"/>s.
    public class Robber : IGameItem
    {
        public Robber(int id)
        {
            Id = id;
        }

        /// <inheritdoc />
        public int Id { get; }

        public Location Location { get; set; }
    }
}
