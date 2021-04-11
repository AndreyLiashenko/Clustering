using Csv;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Clustering.Helpers
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

        public static List<List<double>> TransformRows(this List<ICsvLine> lines)
        {
            List<List<double>> points = new List<List<double>>();
            for (int i = 0; i < lines.FirstOrDefault().ColumnCount; ++i)
            {
                points.Add(new List<double>());
            }

            foreach (var line in lines)
            {
                int i = 0;
                foreach (var point in line.Values)
                {
                    var doublePoints = points[i];
                    if (!string.IsNullOrEmpty(point))
                        doublePoints.Add(double.Parse(point, CultureInfo.InvariantCulture));
                    else doublePoints.Add(float.NaN);
                    ++i;
                }
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
