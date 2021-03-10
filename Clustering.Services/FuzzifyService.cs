using Clustering.Common.Helpers;
using Clustering.Models.Request;
using Clustering.Services.Contracts;
using FLS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clustering.Services
{
    public class FuzzifyService : IFuzzifyService
    {
        public List<Dictionary<string, string>> Get(VariableWithData model)
        {
            var result = new List<Dictionary<string, string>>();
            var variables = new List<LinguisticVariable>();

            //map linguistic variables with their terms
            foreach (var variable in model.LinguisticVariables)
            {
                var newVariable = new LinguisticVariable(variable.Name);
                foreach (var term in variable.Terms)
                {
                    newVariable.MembershipFunctions.AddGaussian(term.Name, term.MathWaiting, term.Sigma);
                }

                variables.Add(newVariable);
            }

            // line in csv file
            foreach (var line in model.Data)
            {
                // var rule = new DynObject();
                var properties = new Dictionary<string, string>();
                // point in line
                foreach (var point in line)
                {
                    // linguistic variable
                    var variable = variables.First(x => x.Name == point.Key);
                    var listOfFuzzifyPoints = new List<FuzzifyPoint>();

                    //term(function)
                    foreach (var function in variable.MembershipFunctions)
                    {
                        var fuzzifyPoint = new FuzzifyPoint();
                        var termValue = function.Fuzzify(point.Value);
                        fuzzifyPoint.TermName = function.Name;
                        fuzzifyPoint.Value = termValue;
                        listOfFuzzifyPoints.Add(fuzzifyPoint);
                    }

                    properties.Add(variable.Name, listOfFuzzifyPoints.GetMaxTerm());
                }

                result.Add(properties);
            }

            return result;
        }
    }
}
