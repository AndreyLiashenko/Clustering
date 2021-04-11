﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clustering.Models.Request
{
    public class Term
    {
        public string Name { get; set; }

        public double MathWaiting { get; set; }

        public double Sigma { get; set; }
    }
}
