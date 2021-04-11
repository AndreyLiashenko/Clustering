using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clustering.Algorithms
{
    public class AlgorithmsUtils
    {
        /// <summary>
        /// First initilization centroid of clusters.
        /// </summary>
        /// <param name="coordinates">points</param>
        /// <param name="numberOfClusters">Number of the cluster</param>
        /// <returns>Cluster center coordinates</returns>
        public static List<List<double>> MakeInitialSeeds(List<List<double>> coordinates, int numberOfClusters)
        {
            Random random = new Random();
            List<List<double>> coordinatesCopy = coordinates.ToList();
            List<List<double>> initialClusterCenters = new List<List<double>>();
            for (int i = 0; i < numberOfClusters; i++)
            {
                int clusterCenterPointNumber = random.Next(0, coordinatesCopy.Count);
                initialClusterCenters.Add(coordinatesCopy[clusterCenterPointNumber]);
                coordinatesCopy.RemoveAt(clusterCenterPointNumber);
            }

            return initialClusterCenters;
        }

        /// <summary>
        /// This method calculates the Euclidean distance from a point to all cluster centers
        /// </summary>
        /// <param name="points">Coordinates of point</param>
        /// <param name="clusterCenters">Cluster center coordinates</param>
        /// <returns>"point coordinate" -> "distance to all cluster centers"</returns>
        public static Dictionary<List<double>, List<double>> CalculateDistancesToClusterCenters(List<List<double>> points, List<List<double>> clusterCenters)
        {
            Dictionary<List<double>, List<double>> map = new Dictionary<List<double>, List<double>>();

            foreach (List<double> pointCoordinates in points)
            {
                List<double> distancesToCenters = new List<double>();
                foreach (List<double> clusterCenter in clusterCenters)
                {
                    double distance = 0;
                    for (int i = 0; i < pointCoordinates.Count; i++)
                    {
                        distance += Math.Pow(pointCoordinates[i] - clusterCenter[i], 2);
                    }
                    distance = Math.Sqrt(distance);
                    distancesToCenters.Add(distance);
                }
                map.Add(pointCoordinates, distancesToCenters);
            }

            return map;
        }
    }
}
