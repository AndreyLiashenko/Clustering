using Clustering.Models;
using Csv;
using System.Collections.Generic;

namespace Clustering.Services.Contracts
{
	public interface IWriterToCsvFile
	{
		byte[] CleansingDataWriteToFile(string path, List<DataVector> data);
	}
}
