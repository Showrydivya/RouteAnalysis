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
            //TODO: Exception Handling
            //TODO: Unit Testing
            //TODO: Commenting
            try
            {
                //Fetch source Data
                VesselsInfo vesselsInfo = JsonConvert.DeserializeObject<VesselsInfo>(File.ReadAllText(jsonFilePath));

                //Analyze routes and fetch intersection points  for vessel paths
                IntersectionPoints pointsOfIntersection = RouteAnalysis.AnalyzeRoutes(vesselsInfo);

                if (pointsOfIntersection != null && pointsOfIntersection.intersections != null)
                {
                    //List Warnings for the intersection points
                    foreach (IntersectionPoint intersection in pointsOfIntersection.intersections)
                    {
                        Console.WriteLine(string.Format("WARNING: Vessel {0} and Vessel {1} intersect at ({2}, {3}) between {4} and {5}",
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
