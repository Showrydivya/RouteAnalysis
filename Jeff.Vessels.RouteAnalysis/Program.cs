using Jeff.Vessels.Data;
using Jeff.Vessels.BL.RouteAnalysis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeff.Vessels.RouteAnalysisTool
{
    class Program
    {
        public const string jsonFilePath = @".\TestData.json";

        static void Main(string[] args)
        {
            try
            {
                //Fetch source Data
                VesselsInfo vesselsInfo = JsonConvert.DeserializeObject<VesselsInfo>(File.ReadAllText(jsonFilePath));

                //Iterate through each position and calculate Distance and Speed between each position
                RouteAnalysis.GetDistanceAndSpeed(ref vesselsInfo);

                //Display Average Speed and Total Distance
                vesselsInfo.Vessels.ForEach(vessel =>
                {
                    Console.WriteLine("Vessel Name: '{0}'", vessel.Name);
                    Console.WriteLine("Average Speed: {0} kmph", Math.Round(vessel.Positions.Average(p => p.SpeedFromPrevPosition), 2));
                    Console.WriteLine("Total Distance Travelled: {0} km", Math.Round(vessel.Positions.Sum(p => p.DistanceToPrevPosition), 2));
                });

                //Analyze routes and fetch intersection points  for vessel paths
                IntersectionPoints pointsOfIntersection = RouteAnalysis.AnalyzeRoutes(vesselsInfo);

                if (pointsOfIntersection != null && pointsOfIntersection.intersections != null && 
                    pointsOfIntersection.intersections.Count > 0)
                {
                    //List Warnings for the intersection points
                    foreach (IntersectionPoint intersection in pointsOfIntersection.intersections)
                    {
                        Console.WriteLine(string.Format("WARNING: Vessel '{0}' and Vessel '{1}' intersect at ({2}, {3})." + 
                            "Vessel '{0}' arrives to the Intersection at {4} and Vessel '{1}' arrives to the intersection at {5}",
                            intersection.Vessel1Name, intersection.Vessel2Name, intersection.XCoordinate, intersection.YCoordinate,
                            intersection.Vessel1ReachIntersection, intersection.Vessel2ReachIntersection));
                    }
                }
                else
                    Console.WriteLine("No WARNINGS on intersections!");
            }
            //Exception Handling
            catch(FileNotFoundException fileNotFound)
            {
                Console.WriteLine("ERROR: FILE NOT FOUND: {0}. \n Details: {1}", fileNotFound.FileName, fileNotFound.ToString());
            }
            catch(NullReferenceException nullException)
            {
                Console.WriteLine("ERROR: NULL REFERENCE: {0}. Details: {1}", nullException.Source, nullException.ToString());
            }
            catch(Exception exception)
            {
                Console.WriteLine("EXCEPTION OCCURED: {0}", exception.ToString());
            }
            Console.ReadLine();
        }

        
    }
}
