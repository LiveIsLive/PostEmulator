using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.HttpClientPerformer.Models
{
	public class Response
	{
		public string Headers { get; protected set; }

		public byte[] Content { get; protected set; }

		public System.Text.Encoding Encoding { get; set; } = System.Text.Encoding.UTF8;

		private string _TextContent;
		public string TextContent
		{
			get
			{
				if (this._TextContent == null)
					this._TextContent = this.Encoding.GetString(this.Content);
				return this._TextContent;
			}
		}

		public Response(string headers, byte[] content)
		{
			this.Headers = headers;
			this.Content = content;
		}
	}
}