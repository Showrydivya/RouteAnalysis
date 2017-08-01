using Jeff.Vessels.Data;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Jeff.Vessels.RouteAnalysisTool
{
    class Program
    {
        public const string jsonFilePath = @"c:\users\showry's\documents\visual studio 2017\Projects\Jeff.Vessels.RouteAnalysisTool\Jeff.Vessels.RouteAnalysisTool\TestData.json";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //Fetch source Data
            VesselsInfo vesselsInfo = JsonConvert.DeserializeObject<VesselsInfo>(File.ReadAllText(jsonFilePath));

            GetDistanceAndSpeed(ref vesselsInfo);

            Console.WriteLine("Hello World!");
        }

        /// <summary>
        /// Iterate through each position and calculate Distance and Speed between each position
        /// </summary>
        /// <param name="vesselsInfo"></param>
        private static void GetDistanceAndSpeed(ref VesselsInfo vesselsInfo)
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
    }
}