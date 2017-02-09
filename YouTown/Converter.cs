namespace YouTown
{
    public class Converter
    {
        public LocationData ToLocationData(Location location) => new LocationData
           {
               X = location.X,
               Y = location.Y,
               Z = location.Z
           }; 

        public Location FromData(LocationData data) =>
            new Location(data.X, data.Y, data.Z);

        public EdgeData ToEdgeData(Edge edge) =>
            new EdgeData
            {
                Location1 = ToLocationData(edge.Location1),
                Location2 = ToLocationData(edge.Location2),
            };

        public Edge FromData(EdgeData data) =>
            new Edge(FromData(data.Location1), FromData(data.Location2));
    }
}
