using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeff.Vessels.Data
{
    //Class representation of Vessels Information (input) and Intersection Points(output)
    //Used for serializing or de-serializing input and output from/to any source

    public class VesselsInfo
    {
        public List<VesselInfo> Vessels { get; set; }
    }

    public class VesselInfo
    {
        public string Name { get; set; }

        public List<VesselPosition> Positions { get; set; }
    }

    public class VesselPosition
    {
        [JsonProperty("x")]
        public int XCoordinate { get; set; }

        [JsonProperty("y")]
        public int YCoordinate { get; set; }

        [JsonProperty("timestamp")]
        public DateTime TimeStamp { get; set; }

        public double DistanceToPrevPosition { get; set; }

        public double SpeedFromPrevPosition { get; set; }
    }

    public class IntersectionPoints
    {
        public List<IntersectionPoint> intersections { get; set; }
    }

    public class IntersectionPoint
    {
        public double XCoordinate { get; set; }

        public double YCoordinate { get; set; }

        public string Vessel1Name { get; set; }

        public string Vessel2Name { get; set; }

        public DateTime Vessel1ReachIntersection { get; set; }

        public DateTime Vessel2ReachIntersection { get; set; }
    }
}
