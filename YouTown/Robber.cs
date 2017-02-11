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

        public Robber(RobberData data)
        {
            Id = data.Id;
            Location = data.Location != null ? new Location(data.Location) : null;
        }

        /// <inheritdoc />
        public int Id { get; }

        public Location Location { get; set; }

        public RobberData ToData() =>
            new RobberData
            {
                Id = Id,
                Location = Location?.ToData()
            };
    }
}
