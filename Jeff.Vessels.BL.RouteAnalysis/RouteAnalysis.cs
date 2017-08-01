using Jeff.Vessels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeff.Vessels.BL.RouteAnalysis
{
    public class RouteAnalysis
    {
        /// <summary>
        /// Analyse the routes of given Vessels and determine the intersection points, if any.
        /// Return the Intersection Points where two vessel reach intersection within one hour
        /// </summary>
        /// <param name="vesselsInfo"></param>
        /// <returns></returns>
        public static IntersectionPoints AnalyzeRoutes(VesselsInfo vesselsInfo)
        {
            IntersectionPoints pointsOfIntersection = new IntersectionPoints { intersections = new List<IntersectionPoint>() };

            try
            {
                //Determine the intersection points for each vessel path
                //iterating through vessel paths
                for (int i = 0; i < vesselsInfo.Vessels.Count; i++)
                {
                    VesselInfo previousVessel = vesselsInfo.Vessels[i];

                    for (int j = i + 1; j < vesselsInfo.Vessels.Count; j++)
                    {
                        VesselInfo vessel = vesselsInfo.Vessels[j];

                        VesselPosition pv_PreviousPosition = default(VesselPosition);
                        bool pv_FirstPosition = true;
                        foreach (VesselPosition pv_Position in previousVessel.Positions)
                        {
                            if (pv_FirstPosition)
                                pv_FirstPosition = false;
                            else
                            {
                                VesselPosition v_PreviousPosition = default(VesselPosition);
                                bool v_FirstPosition = true;
                                foreach (VesselPosition v_Position in vessel.Positions)
                                {
                                    if (v_FirstPosition)
                                        v_FirstPosition = false;
                                    else
                                    {
                                        //Check if there is an Intersection between the paths
                                        Vector intersection = new Vector();
                                        bool intersect = LineSegment.LineSegementsIntersect(
                                            new Vector(pv_PreviousPosition.XCoordinate, pv_PreviousPosition.YCoordinate),
                                            new Vector(pv_Position.XCoordinate, pv_Position.YCoordinate),
                                            new Vector(v_PreviousPosition.XCoordinate, v_PreviousPosition.YCoordinate),
                                            new Vector(v_Position.XCoordinate, v_Position.YCoordinate), out intersection, true);

                                        if (intersect)
                                        {
                                            //If an intersection is found, add it to the list of Intersection points
                                            pointsOfIntersection.intersections.Add(
                                                new IntersectionPoint
                                                {
                                                    XCoordinate = intersection.X,
                                                    YCoordinate = intersection.Y,
                                                    Vessel1Name = previousVessel.Name,
                                                    Vessel2Name = vessel.Name,
                                                    Vessel1ReachIntersection = GetTimeStamp(pv_PreviousPosition, intersection, pv_Position.SpeedFromPrevPosition),
                                                    Vessel2ReachIntersection = GetTimeStamp(v_PreviousPosition, intersection, v_Position.SpeedFromPrevPosition)
                                                });
                                        }

                                    }
                                    v_PreviousPosition = v_Position;
                                }
                            }
                            pv_PreviousPosition = pv_Position;
                        }
                    }
                }
                        

                //Iterate through Intersection Points and display warning only to those where the vessels reach intersection within an hour
                pointsOfIntersection = new IntersectionPoints
                {
                    intersections = pointsOfIntersection.intersections.Where(i => (i.Vessel1ReachIntersection - i.Vessel2ReachIntersection).Hours < 1).ToList()
                };
            }
            catch { throw; }

            return pointsOfIntersection;
        }

        /// <summary>
        /// Get Time when the vessel reaches a given point of intersection
        /// given the starting point, intersection point and speed of the vessel
        /// </summary>
        /// <param name="PreviousPosition"></param>
        /// <param name="intersection"></param>
        /// <param name="speedFromPrevPosition"></param>
        /// <returns></returns>
        private static DateTime GetTimeStamp(VesselPosition PreviousPosition, Vector intersection, double speedFromPrevPosition)
        {
            double timeToReachIntersection = GetDistance(PreviousPosition.XCoordinate, PreviousPosition.YCoordinate,
                intersection.X, intersection.Y) / speedFromPrevPosition;

            return PreviousPosition.TimeStamp.AddHours(timeToReachIntersection);
        }

        /// <summary>
        /// Iterate through each position and calculate Distance and Speed between each position
        /// </summary>
        /// <param name="vesselsInfo"></param>
        public static void GetDistanceAndSpeed(ref VesselsInfo vesselsInfo)
        {
            foreach (VesselInfo vessel in vesselsInfo.Vessels)
            {
                VesselPosition previousPosition = default(VesselPosition);
                bool firstPosition = true;
                foreach (VesselPosition position in vessel.Positions)
                {
                    if (firstPosition)
                        firstPosition = false;
                    else
                    {
                        position.DistanceToPrevPosition = GetDistance(previousPosition.XCoordinate,
                            previousPosition.YCoordinate, position.XCoordinate, position.YCoordinate);
                        position.SpeedFromPrevPosition = GetSpeed(position.DistanceToPrevPosition, position.TimeStamp - previousPosition.TimeStamp);
                    }
                    previousPosition = position;
                }
            }
        }

        /// <summary>
        /// Return speed given the distance and Time between two points
        /// </summary>
        /// <param name="distanceToPrevPosition"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        private static double GetSpeed(double distanceToPrevPosition, TimeSpan timeSpan)
        {
            return distanceToPrevPosition / timeSpan.Minutes * 60;
        }

        /// <summary>
        /// Return Distance between two points
        /// </summary>
        /// <param name="xCoordinate1"></param>
        /// <param name="yCoordinate1"></param>
        /// <param name="xCoordinate2"></param>
        /// <param name="yCoordinate2"></param>
        /// <returns></returns>
        private static double GetDistance(double xCoordinate1, double yCoordinate1, double xCoordinate2, double yCoordinate2)
        {
            return Math.Sqrt(Math.Pow(xCoordinate2 - xCoordinate1, 2) + Math.Pow(yCoordinate2 - yCoordinate1, 2));
        }
    }
}
