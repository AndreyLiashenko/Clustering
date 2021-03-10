using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clustering.Algorithms
{
    public class ListUtils
    {
        public static bool IsListEqualsToAnother(List<double> list1, List<double> list2)
        {
            if (list1.Count != list2.Count)
            {
                return false;
            }

            for (int i = 0; i < list1.Count; i++)
            {              
                if (list1[i] != list2[i])
                {
                    return false;
                }
            }

            return true;
        }
        public static bool IsListEqualsToAnother(List<List<double>> list1, List<List<double>> list2)
        {
            if (list1.Count != list2.Count)
            {
                return false;
            }

            for (int i = 0; i < list1.Count; i++)
            {
                if (list1[i].Count != list2[i].Count)
                {
                    return false;
                }
            }

            for (int i = 0; i < list1.Count; i++)
            {
                for (int j = 0; j < list1[i].Count; j++)
                {
                    if (list1[i][j] != list2[i][j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static int GetMinIndex(List<double> values)
        {
            double min = double.MaxValue;
            int minIndex = 0;
            for (int i = 0; i < values.Count; i++)
            {
                if (values[i] < min)
                {
                    min = values[i];
                    minIndex = i;
                }
            }

            return minIndex;
        }
        
        public static int GetMaxIndex(List<double> values)
        {
            double max = double.MinValue;
            int maxIndex = 0;
            for (int i = 0; i < values.Count; i++)
            {
                if (values[i] > max)
                {
                    max = values[i];
                    maxIndex = i;
                }
            }

            return maxIndex;
        }

        public static double GetMaxElement(List<List<double>> values)
        {
            double max = double.MinValue;
            for (int i = 0; i < values.Count; i++)
            {
                for (int j = 0; j < values[0].Count; j++)
                {
                    if (values[i][j] > max)
                    {
                        max = values[i][j];
                    }
                }

            }

            return max;
        }
        
        public static List<List<double>> CreateDifferencesMatrix(List<List<double>> matrix1, List<List<double>> matrix2)
        {
            List<List<double>> differences = new List<List<double>>();
            for (int i = 0; i < matrix1.Count; i++)
            {
                List<double> rowDifferences = new List<double>();
                for (int j = 0; j < matrix1[0].Count; j++)
                {
                    double result = Math.Abs(matrix1[i][j] - matrix2[i][j]);
                    rowDifferences.Add(result);
                }

                differences.Add(rowDifferences);
            }

            return differences;
        }
        public static int GetElementIndex(List<List<double>> list, List<double> element)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (IsListEqualsToAnother(list[i], element))
                {
                    return i;
                }
            }

            return -1;
        }

    }
}
