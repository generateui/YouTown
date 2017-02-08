using System;
using System.Collections.Generic;
using System.Linq;

namespace YouTown
{
    public interface IBoard
    {
        IReadOnlyDictionary<Location, IHex> HexesByLocation { get; }
        IDictionary<Edge, Road> RoadsByEdge { get; } 
        IDictionary<Point, Town> TownsByPoint { get; } 
        IDictionary<Point, City> CitiesByPoint { get; }
        IDictionary<Point, IPointPiece> PiecesByPoint { get; } 
        IDictionary<Edge, IEdgePiece> PiecesByEdge { get; }
        ISet<IProducer> Producers { get; }
        ISet<IPiece> Pieces { get; }
        Robber Robber { get; } // TODO: does this belong to IGame or IBoard?

        /// <summary>
        /// Uses this instance of board to create a new instance where placeholders for 
        /// hexes, chits and ports are replaced by concrete hexes, chits and ports.
        /// </summary>
        /// <returns>A new instance of type of self</returns>
        IBoard Setup(IPortList ports, IChitList chits, IHexList hexList, IRandom random);
    }

    /// <summary>
    /// Board with X concentric circles bordered by a circle of water
    /// </summary>
    public class ConcentricBoard : IBoard
    {
        private Dictionary<Location, IHex> _hexesbyLocation = new Dictionary<Location, IHex>();

        private ConcentricBoard(Dictionary<Location, IHex> hexesByLocation)
        {
            _hexesbyLocation = new Dictionary<Location, IHex>(hexesByLocation);
        }

        /// <summary>
        /// Creates a new board in a concentric form
        /// </summary>
        /// Places concentric circles of <see cref="RandomHex"/> instances. The outer circle 
        /// will contain <see cref="Water"/> hexes.
        /// The standard board is a concentric board made up of 4 levels. It contains a
        /// center hex with 6 neghboring hexes, with another concentric hexagonic cirle with 
        /// land hexes. Finally, a concentric hexagonic circle with water hexes at level 4 is 
        /// placed.
        /// <param name="concentricCircles">
        /// Amount of circles used to build the board with. Must be 3 or greater and 10 or less.
        /// </param>
        public ConcentricBoard(int concentricCircles)
        {
            if (concentricCircles > 10 || concentricCircles < 2)
            {
                throw new ArgumentException(nameof(concentricCircles));
            }
            // Generate locations
            var center = new Location(0, 0, 0);
            var locations = new List<Location> {center};
            for (int i = 0; i < concentricCircles -1; i++)
            {
                var locationsToAdd = locations.SelectMany(l => l.Neighbors).ToList();
                locations.AddRange(locationsToAdd);
            }
            locations = locations.Distinct().ToList();

            // Divide water and land hexes
            var maxAxis = concentricCircles - 1;
            Func<Location, bool> isAtOuterCircle = l =>
                l.X == maxAxis || l.Y == maxAxis || l.Z == maxAxis ||
                l.X == -maxAxis || l.Y == -maxAxis || l.Z == -maxAxis;
            var waterLocations = locations.Where(isAtOuterCircle);
            var randomLocations = locations.Except(waterLocations);
            foreach (var randomLocation in randomLocations)
            {
                _hexesbyLocation[randomLocation] = new RandomHex(Identifier.DontCare, randomLocation, null);
            }

            //assign ports: every other 3 edge on side
            // get all edges
            // filter on water location
            // loop over, place next connected edge
            // skip 2
            var waterEdges = waterLocations.SelectMany(wl => wl.Edges).Distinct();
            var landEdges = randomLocations.SelectMany(rl => rl.Edges).Distinct();
            var portEdges = waterEdges.Intersect(landEdges);
            var edge = portEdges.FirstOrDefault();
            var sortedPortEdges = new List<Edge> {edge};
            // sort all edges into one continuous string of edges
            while (sortedPortEdges.Count < portEdges.Count())
            {
                var connectingPortEdge = portEdges.FirstOrDefault(pe => edge.Connects(pe) && !sortedPortEdges.Contains(pe));
                sortedPortEdges.Add(connectingPortEdge);
                edge = connectingPortEdge;
            }
            var edgesToPutPortOn = new List<Edge>();
            for (int i = 0; i < sortedPortEdges.Count; i++)
            {
                if (i%3 == 0)
                {
                    var edgeToPutPortOn = sortedPortEdges[i];
                    edgesToPutPortOn.Add(edgeToPutPortOn);
                }
            }
            foreach (var waterLocation in waterLocations)
            {
                RandomPort port = null;
                var portLocations = waterLocation.Edges.Intersect(edgesToPutPortOn);
                bool addAPort = portLocations.Any();
                if (addAPort)
                {
                    var portLocation = portLocations.FirstOrDefault();
                    var landLocation = portLocation.Location1.Equals(waterLocation) ? portLocation.Location2 : portLocation.Location1;
                    port = new RandomPort(Identifier.DontCare, waterLocation, landLocation);
                }
                _hexesbyLocation[waterLocation] = new Water(Identifier.DontCare, waterLocation, port);
            }
        }

        public ISet<IProducer> Producers { get; }
        public ISet<IPiece> Pieces { get; }
        public Robber Robber { get; }
        public IReadOnlyDictionary<Location, IHex> HexesByLocation => _hexesbyLocation;
        public IDictionary<Edge, Road> RoadsByEdge { get; }
        public IDictionary<Point, Town> TownsByPoint { get; }
        public IDictionary<Point, City> CitiesByPoint { get; }
        public IDictionary<Point, IPointPiece> PiecesByPoint { get; }
        public IDictionary<Edge, IEdgePiece> PiecesByEdge { get; }

        public int ConcentricCircles { get; set; }

        /// <summary>
        /// Produces a playable board given the design of the current board
        /// </summary>
        /// <param name="ports">
        /// Given ports are used to replace the random ports with
        /// </param>
        /// <param name="chits">
        /// Chits used to place on empty hexes which are allowed to have chits
        /// </param>
        /// <param name="hexList">
        /// Hexes used to replace the <see cref="RandomHex"/> instances with
        /// </param>
        /// <param name="random">
        /// Randomizer instance used to pick a random item from a portlist, chitlist or hexlist
        /// </param>
        /// <returns>
        /// A new board instance ready to play on
        /// </returns>
        /// TODO: move the different algorithms to separate classes for reuse
        public IBoard Setup(IPortList ports, IChitList chits, IHexList hexList, IRandom random)
        {
            var board = new ConcentricBoard(_hexesbyLocation);
            // replace randomports with ports
            // if not sufficient concrete ports are delivered, remove the port
            var portsToReplace = board._hexesbyLocation.Values.Where(h => h.Port != null).ToList();
            var replacedPorts = new List<Water>();
            var portz = new List<IPort>(ports);
            while (portz.Any() && portsToReplace.Any())
            {
                var portToReplace = portsToReplace.PickRandom(random);
                portsToReplace.Remove(portToReplace);
                var replacement = portz.PickRandom(random);
                portz.Remove(replacement);
                var replaced = new Water(portToReplace.Id, portToReplace.Location, replacement);
                replacedPorts.Add(replaced);
            }
            replacedPorts.ForEach(rp => board._hexesbyLocation[rp.Location] = rp);
            // TODO: report port count mismatch
            // Probably want to do some reporting here when the counts differ
            // e.g. "3 port replacements left", "4 port replacements short, did not place port"
            foreach (var hex in portsToReplace)
            {
                var replaced = new Water(hex.Id, hex.Location, port: null);
                board._hexesbyLocation[hex.Location] = replaced;
            }

            // replace randomhexes with hexes
            var hexesToReplace = board._hexesbyLocation.Values.Where(h => h.IsRandom).ToList();
            var replacedHexes = new List<IHex>();
            var hexez = new List<IHex>(hexList.Hexes);
            while (hexez.Any() && hexesToReplace.Any())
            {
                var hexToReplace = hexesToReplace.PickRandom(random);
                hexesToReplace.Remove(hexToReplace);
                var replacement = hexez.PickRandom(random);
                hexez.Remove(replacement);
                var replaced = replacement.Clone(Identifier.DontCare, hexToReplace.Location);
                replacedHexes.Add(replaced);
            }
            replacedHexes.ForEach(rh => board._hexesbyLocation[rh.Location] = rh);
            // replace leftover randomhexes with deserts
            foreach (var hex in hexesToReplace)
            {
                var desert = new Desert(hex.Id, hex.Location);
                board._hexesbyLocation[hex.Location] = desert;
            }
            // TODO: report hex count mismatch

            // replace randomchits with chits
            // TODO: ensure reds dont touch
            var hexesToPlaceChitOn = board._hexesbyLocation.Values.Where(h => h.CanHaveChit);
            var chitz = new List<IChit>(chits.Chits);

            // first place the red chits
            var allowedHexes = hexesToPlaceChitOn.ToList();
            var redChits = chitz.Where(c => c.IsRed).ToList();
            while (redChits.Any() && allowedHexes.Any())
            {
                var redChitToPlace = redChits.PickRandom(random);
                redChits.Remove(redChitToPlace);
                var hex = allowedHexes.PickRandom(random);
                allowedHexes.Remove(hex);
                hex.Chit = redChitToPlace;

                // neighboring hexes cannot also have a red chit, so remove them from
                // hex candidates to put red chit on
                var neighborLocations = hex.Location.Neighbors;
                var neighborHexes = allowedHexes.Where(dh => neighborLocations.Contains(dh.Location)).ToList();
                neighborHexes.ForEach(nh => allowedHexes.Remove(nh));
            }

            hexesToPlaceChitOn = board._hexesbyLocation.Values
                .Where(h => h.CanHaveChit)
                .Where(h => h.Chit == null)
                .ToList();
            var nonRedChits = chitz.Where(c => !c.IsRed).ToList();
            foreach (var hex in hexesToPlaceChitOn)
            {
                if (!nonRedChits.Any())
                {
                    // if we don't have chits we simply stop distributing them
                    break;
                }
                var chitToPlace = nonRedChits.PickRandom(random);
                nonRedChits.Remove(chitToPlace);
                hex.Chit = chitToPlace;
            }
            // TODO: prevent equal chits neighboring each other e.g. 9|9
            // TODO: report chit count mismatch

            return board;
        }

    }
}
