using Clustering.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clustering.InputModel.Rules
{
    public class LinguisticVariableModel
    {
        public string Name { get; set; }

        public List<Term> Terms { get; set; }
    }
}
