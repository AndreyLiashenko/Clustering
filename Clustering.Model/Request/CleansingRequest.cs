using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clustering.Model.Request
{
	public class CleansingRequest
	{
		public IFormFile File { get; set; }
		public string CleanseParams { get; set; }
	}
}
