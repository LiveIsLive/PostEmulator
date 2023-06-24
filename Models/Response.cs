using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.PostEmulator.Models
{
	public class Response : Caliburn.Micro.PropertyChangedBase
	{
		public string Headers { get; protected set; }

		public byte[] Content { get; protected set; }

		public readonly string MediaType;

		protected readonly string CharSet;

		public readonly string FileName;

		public System.Collections.Generic.IList<string> MissedHeaders { get; protected set; }

		public string Error { get; protected set; }

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

		private string _FormattedContentCode;
		public string FormattedContentCode
		{
			get
			{
				if (this._FormattedContentCode == null)
					switch (this.ContentType)
					{
						case ResponseContentType.HTML:
						{
							AngleSharp.Html.Parser.HtmlParser parser = new AngleSharp.Html.Parser.HtmlParser();
							AngleSharp.Html.Dom.IHtmlDocument document = parser.ParseDocument(this.TextContent);
							System.IO.StringWriter writer = new System.IO.StringWriter();
							document.ToHtml(writer, new AngleSharp.Html.PrettyMarkupFormatter());
							this._FormattedContentCode = writer.ToString();
							break;
						}
						case ResponseContentType.Json:
						{
							this._FormattedContentCode = Newtonsoft.Json.JsonConvert.DeserializeObject(this.TextContent).ToString();
							break;
						}
						case ResponseContentType.XML:
						{
							//System.Xml.Linq.XDocument document = System.Xml.Linq.XDocument.Parse(this.TextContent);
							//System.IO.StringWriter writer = new System.IO.StringWriter();
							//document.Save(writer);
							//this._FormattedContentCode = writer.ToString();
							this._FormattedContentCode = System.Xml.Linq.XDocument.Parse(this.TextContent).ToString();
							break;
						}
						default:
							this._FormattedContentCode = this.TextContent;
							break;
					}
				return this._FormattedContentCode;
			}
		}

		private System.IO.MemoryStream _ContentStream;
		public System.IO.MemoryStream ContentStream
		{
			get
			{
				if (this._ContentStream == null)
					this._ContentStream = new System.IO.MemoryStream(this.Content);
				return this._ContentStream;
			}
		}

		private ResponseContentType? _ContentType;
		public ResponseContentType ContentType
		{
			get
			{
				if (this._ContentType == null)
					if (this.MediaType == null)
						this._ContentType = Models.ResponseContentType.Binary;
					else if (this.MediaType.Contains("html"))
						this._ContentType = Models.ResponseContentType.HTML;
					else if (this.MediaType.Contains("json"))
						this._ContentType = Models.ResponseContentType.Json;
					else if (this.MediaType.Contains("javascript"))
						this._ContentType = Models.ResponseContentType.JavaScript;
					else if(this.MediaType.Contains("xml"))
						this._ContentType = Models.ResponseContentType.XML;
					else if(this.MediaType.Contains("image"))
						this._ContentType = Models.ResponseContentType.Image;
					else if(this.MediaType.Contains("text"))
						this._ContentType = Models.ResponseContentType.PlainText;
					else this._ContentType = Models.ResponseContentType.Binary;
				return this._ContentType.Value;
			}
		}

		private bool? _IsCodeContent;
		public bool IsCodeContent
		{
			get
			{
				if(this._IsCodeContent==null)
					switch(this.ContentType)
					{
						case ResponseContentType.HTML:
						case ResponseContentType.CSS:
						case ResponseContentType.JavaScript:
						case ResponseContentType.XML:
						case ResponseContentType.Json:
							this._IsCodeContent = true;
							break;
						default:
							this._IsCodeContent = false;
							break;
					}
				return this._IsCodeContent.Value;
			}
		}

		public Response(string headers, byte[] content,string mediaType,string charSet,string fileName, IList<string> missedHeaders, string error)
		{
			this.Headers = headers;
			this.Content = content;
			this.MediaType = mediaType?.ToLower();
			this.CharSet = charSet;
			this.FileName = fileName;
			this.MissedHeaders = missedHeaders;
			this.Error = error;
		}
	}
}