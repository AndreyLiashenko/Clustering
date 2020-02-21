using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clustering.Algorithms.KMeans
{
    public class KMeansAlgorithm
    {
        public static List<List<double>> RecalculateCoordinateOfClusterCenters(Dictionary<List<double>, List<double>> clusters, List<List<double>> clusterCenters)
        {
            List<List<double>> newClusterCenters = new List<List<double>>();
            foreach (List<double> clusterCenter in clusterCenters)
            {
                var map = clusters.Where(point => ListUtils.IsListEqualsToAnother(point.Value, clusterCenter));

                List<double> sums = new List<double>();
                for (int i = 0; i < clusterCenter.Count; i++)
                {
                    sums.Add(0);
                }

                foreach (KeyValuePair<List<double>, List<double>> point in map)
                {
                    List<double> pointCoordinates = point.Key;
                    for (int i = 0; i < pointCoordinates.Count; i++)
                    {
                        sums[i] += pointCoordinates[i];
                    }
                }

                for (int i = 0; i < sums.Count; i++)
                {
                    sums[i] /= map.Count();
                }

                newClusterCenters.Add(sums);
            }

            return newClusterCenters;
        }
        public static Dictionary<List<double>, List<double>> MakeClusters(List<List<double>> points, List<List<double>> clusterCenters)
        {
            Dictionary<List<double>, List<double>> distancesToClusterCenters = AlgorithmsUtils.CalculateDistancesToClusterCenters(points, clusterCenters);
            Dictionary<List<double>, List<double>> clusters = new Dictionary<List<double>, List<double>>();

            foreach (KeyValuePair<List<double>, List<double>> distanceToClusterCenter in distancesToClusterCenters)
            {
                int clusterNumber = ListUtils.GetMinIndex(distanceToClusterCenter.Value);
                clusters.Add(distanceToClusterCenter.Key, clusterCenters[clusterNumber]);
            }

            return clusters;
        }
    }
}
