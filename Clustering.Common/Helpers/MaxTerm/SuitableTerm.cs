using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clustering.Common.Helpers
{
    public static class SuitableTerm
    {
        public static string GetMaxTerm(this List<FuzzifyPoint> fuzzifyPoints)
        {
            var max = fuzzifyPoints[0];

            for (int i = 1; i < fuzzifyPoints.Count; i++)
            {
                if(fuzzifyPoints[i].Value > max.Value)
                {
                    max = fuzzifyPoints[i];
                }
            }

            return max.TermName;
        }
    }
}
