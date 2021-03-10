using Clustering.Models;
using Clustering.Models.GaussianResponse;
using Clustering.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clustering.Services
{
    public class GaussParameterService : IGaussParameterService
    {
        public List<GaussianModel> Get(Centroids centroids, int axisNumber = 0)
        {
            var listOfCenter = new List<double>();
            var response = new List<GaussianModel>();
            foreach (var cluster in centroids.Centroid)
            {
                int counter = 0;
                foreach (var point in cluster)
                {
                    if (counter == axisNumber)
                    {
                        listOfCenter.Add(point);
                        break;
                    }
                    counter++;
                }
            }

            listOfCenter.Sort();
            for (int i = 0; i < listOfCenter.Count; i++)
            {
                var model = new GaussianModel();
                if (listOfCenter[i] == listOfCenter.Last())
                {
                    var lasItem = response.Last();
                    model.MathWaiting = listOfCenter[i];
                    model.Sigma = lasItem.Sigma;
                    model.SimpleSigma = lasItem.SimpleSigma;
                    response.Add(model);
                    break;
                }

                model.MathWaiting = listOfCenter[i];

                double sigma = Math.Abs((listOfCenter[i] - listOfCenter[i + 1]) / listOfCenter.Count());
                var sigmaTwoStep = 2 * (sigma * sigma);
                model.Sigma = sigmaTwoStep;
                model.SimpleSigma = sigma;
                response.Add(model);
            }

            return response;
        }
    }
}
