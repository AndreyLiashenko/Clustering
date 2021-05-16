using Microsoft.ML.Data;

namespace Clustering.Models
{
    public class DataVector
    {
        public string Label;
        [VectorType]
        public FeatureType[] Features;
    }

    public class FeatureType
    {
        public double Value;
    }
}
