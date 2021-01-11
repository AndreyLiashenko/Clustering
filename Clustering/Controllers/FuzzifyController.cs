using Clustering.Helpers;
using Clustering.Helpers.Dynamic;
using Clustering.Helpers.Fuzzify;
using Clustering.Helpers.MaxTerm;
using Clustering.InputModel.Rules;
using Clustering.Services.CsvRead;
using FLS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Clustering.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class FuzzifyController : ControllerBase
    {
        private readonly ILogger _logger;
        public FuzzifyController(ILogger<FuzzifyController> logger)
        {
            _logger = logger;
        }

        [HttpPost("getRules")]
        public ActionResult<double> Get([FromBody] VariableWithData model)
        {
            var result = new List<dynamic>();
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
                var properties = new Dictionary<string, object>();
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

                result.Add(new DynObject(properties));
            }

            return Ok(result);
        }
    }
}
