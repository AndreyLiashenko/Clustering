using Csv;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clustering.Services.Contracts
{
	public interface IWriterToCsvFile
	{
		byte[] Write(string path, List<ICsvLine> data, List<string> columnNames);
	}
}
