using Clustering.Services.Contracts;
using Csv;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Clustering.Services
{
	public class WriterToCsvFile : IWriterToCsvFile
	{
		public byte[] Write(string path, List<ICsvLine> data, List<string> columnNames)
		{
            using (var memStream = new MemoryStream())
            {
                var byteArray = System.IO.File.ReadAllBytes(path);
                memStream.Write(byteArray, 0, byteArray.Length);
                memStream.Position = 0;
                using (StreamWriter streamWriter = new StreamWriter(memStream))
                {
                    using (CsvHelper.CsvWriter csvWriter = new CsvHelper.CsvWriter(streamWriter, CultureInfo.CurrentCulture))
                    {
                        foreach (var headerColumn in columnNames)
                        {
                            csvWriter.WriteField(headerColumn);
                        }

                        csvWriter.NextRecord();

                        foreach (var item in data)
                        {
                            foreach (var str in item.Values)
                            {
                                csvWriter.WriteField(str);
                            }
                            csvWriter.NextRecord();

                        }
                    }
                }

                return memStream.ToArray();
            }
        }
	}
}
