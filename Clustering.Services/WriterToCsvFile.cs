using Clustering.Models;
using Clustering.Services.Contracts;
using Csv;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Clustering.Services
{
	public class WriterToCsvFile : IWriterToCsvFile
	{
		public byte[] CleansingDataWriteToFile(string path, List<DataVector> data)
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
						foreach (var headerColumn in data)
						{
							csvWriter.WriteField(headerColumn.Label);
						}

						csvWriter.NextRecord();

                        for(int i = 0; i < data.FirstOrDefault().Features.Length; i++)
						{
                            for (int j = 0; j < data.Count; j++)
                            {
                                csvWriter.WriteField(data[j].Features[i].Value.ToString());
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
