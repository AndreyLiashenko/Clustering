using Clustering.Models.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clustering.Services.Contracts
{
    public interface IFuzzifyService
    {
        List<Dictionary<string, string>> Get(VariableWithData model);
    }
}
