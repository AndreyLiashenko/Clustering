using Csv;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Clustering.Common.Extensions
{
    public static class StringListToDoubleList
    {
        /// <summary>
        /// Transform list of strings to list of double.
        /// </summary>
        /// <param name="lines">List of string from csv file.</param>
        /// <returns>Lists of lists of double</returns>
        public static List<List<double>> Transform(this List<ICsvLine> lines)
        {
            List<List<double>> points = new List<List<double>>();
            foreach (var line in lines)
            {
                List<double> doublePoints = new List<double>();
                foreach (var point in line.Values)
                {
                    if (!string.IsNullOrEmpty(point))
                        doublePoints.Add(double.Parse(point, CultureInfo.InvariantCulture));
                    else doublePoints.Add(float.NaN);
                }
                points.Add(doublePoints);
            }

            return points;
        }


        /// <summary>
        /// Transform list of strings to list of double.
        /// </summary>
        /// <param name="lines">List of string from csv file.</param>
        /// <returns>Lists of lists of double</returns>
        //public static List<List<double?>> TransformNullable(this List<ICsvLine> lines)
        //{
        //    List<List<double?>> points = new List<List<double?>>();
        //    foreach (var line in lines)
        //    {
        //        List<double?> doublePoints = new List<double?>();
        //        foreach (var point in line.Values)
        //        {
        //            if (!string.IsNullOrEmpty(point))
        //                doublePoints.Add(Convert.ToDouble(point));
        //            else doublePoints.Add(null);
        //        }
        //        points.Add(doublePoints);
        //    }

        //    return points;
        //}
    }
}
