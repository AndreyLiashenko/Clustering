using Microsoft.ML.Data;

namespace Clustering.Models
{
    public class DataVector
    {
        public double Label;
        [VectorType]
        public double[] Features;
    }
}
