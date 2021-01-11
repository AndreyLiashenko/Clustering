using System;
using System.Collections.Generic;
using System.Text;

namespace Clustering.Models
{
    public class CleanseParameters
    {
        public bool AutoCleansing { get; set; }
        public bool FilterData { get; set; }
        public bool ReplaceMissingValue { get; set; }
        public bool NormalizeData { get; set; }
        public bool RemoveDublicates { get; set; }
    }
}
