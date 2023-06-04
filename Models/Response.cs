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

		protected readonly string MediaType;

		protected readonly string CharSet;

		protected readonly string FileName;

		private System.Text.Encoding _Encoding;
		public System.Text.Encoding Encoding
		{
			get
			{
				if(this._Encoding==null)
					try
					{
						this._Encoding = System.Text.Encoding.GetEncoding(this.CharSet);
					}
					catch
					{
						this._Encoding = System.Text.Encoding.UTF8;
					}
				return this._Encoding;
			}
		}

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

		private ContentType? _ContentType;
		public ContentType? ContentType
		{
			get
			{
				if (this._ContentType == null)
					if (this.MediaType == null)
						this._ContentType = Models.ContentType.Binary;
					else if (this.MediaType.Contains("html"))
						this._ContentType = Models.ContentType.Html;
					else if (this.MediaType.Contains("json"))
						this._ContentType = Models.ContentType.Json;
					else if(this.MediaType.Contains("xml"))
						this._ContentType = Models.ContentType.Xml;
					else if(this.MediaType.Contains("image"))
						this._ContentType = Models.ContentType.Image;
					else if(this.MediaType.Contains("text"))
						this._ContentType = Models.ContentType.Image;
					else this._ContentType = Models.ContentType.Binary;
				return this._ContentType;
			}
		}

		public Response(string headers, byte[] content,string mediaType,string charSet,string fileName)
		{
			this.Headers = headers;
			this.Content = content;
			this.MediaType = mediaType?.ToLower();
			this.CharSet = charSet;
			this.FileName = fileName;
		}
	}
}