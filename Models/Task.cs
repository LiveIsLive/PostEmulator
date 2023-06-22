using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;

namespace ColdShineSoft.HttpClientPerformer.Models
{
	public class Task : Caliburn.Micro.PropertyChangedBase
	{
		private string _Url;
		[Newtonsoft.Json.JsonProperty]
		public virtual string Url
		{
			get
			{
				return this._Url;
			}
			set
			{
				this._Url = value;
				this._Uri = null;

				if (this._UrlParameters == null)
					return;
				int index = this._Url.IndexOf("?");
				if (index >= 0)
					this.ReplaceSelectedOldItems(this._UrlParameters, this.StringToUrlParameters(this._Url.Substring(index + 1)).ToArray());
				else foreach (RequestItem parameter in this._UrlParameters)
						parameter.Selected = false;
			}
		}

		private System.Uri _Uri;
		public System.Uri Uri
		{
			get
			{
				if (this._Uri == null)
					this._Uri = new Uri(this.Url);
				return this._Uri;
			}
		}

		private System.Collections.ObjectModel.ObservableCollection<RequestItem> _UrlParameters;
		[Newtonsoft.Json.JsonProperty(ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Replace)]
		public System.Collections.ObjectModel.ObservableCollection<RequestItem> UrlParameters
		{
			get
			{
				if (this._UrlParameters == null)
				{
					if(this._Url==null)
						this._UrlParameters = new System.Collections.ObjectModel.ObservableCollection<RequestItem>();
					else
					{
						int index = this.Url.IndexOf("?");
						if (index >= 0)
							this._UrlParameters = new System.Collections.ObjectModel.ObservableCollection<RequestItem>(this.StringToUrlParameters(this._Url.Substring(index + 1)));
						else this._UrlParameters = new System.Collections.ObjectModel.ObservableCollection<RequestItem>();
						foreach (RequestItem item in this._UrlParameters)
							item.PropertyChanged += UrlParameter_PropertyChanged;
					}
					this._UrlParameters.CollectionChanged += UrlParameters_CollectionChanged;
				}
				return this._UrlParameters;
			}
			set
			{
				this._UrlParameters = value;
				foreach (RequestItem item in this._UrlParameters)
					item.PropertyChanged += UrlParameter_PropertyChanged;
				this._UrlParameters.CollectionChanged += UrlParameters_CollectionChanged;
			}
		}

		private void UrlParameters_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null)
				foreach (RequestItem item in e.NewItems)
					item.PropertyChanged += UrlParameter_PropertyChanged;
			this.UrlParameter_PropertyChanged(sender, null);
		}

		private void UrlParameter_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (this._Url == null)
				return;
			int index = this._Url.IndexOf("?");
			if (index == -1)
				this._Url += "?";
			else this._Url = this._Url.Substring(0,index + 1);
			RequestItem[] parameters = this.UrlParameters.Where(p => p.Selected).ToArray();
			if (parameters.Length == 0)
				this._Url = this._Url.Substring(0, this._Url.Length - 1);
			else this._Url += string.Join("&", parameters.Select(i => $"{i.Name}={System.Net.WebUtility.UrlEncode(i.Value)}"));

			this.NotifyOfPropertyChange(() => this.Url);
		}

		protected System.Collections.Generic.IEnumerable<RequestItem> StringToUrlParameters(string s)
		{
			RequestItem[] parameters = this.StringToRequestItems(s, '&', '=').ToArray();
			foreach (RequestItem parameter in parameters)
			{
				parameter.Value = System.Net.WebUtility.UrlDecode(parameter.Value);
				parameter.ToString();
			}
			return parameters;
		}

		private System.Collections.ObjectModel.ObservableCollection<RequestItem> _HeaderItems;
		[Newtonsoft.Json.JsonProperty(ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Replace)]
		public System.Collections.ObjectModel.ObservableCollection<RequestItem> HeaderItems
		{
			get
			{
				if(this._HeaderItems==null)
				{
					if (this._HeaderText == null)
						this._HeaderItems = new System.Collections.ObjectModel.ObservableCollection<RequestItem>();
					else this._HeaderItems = new System.Collections.ObjectModel.ObservableCollection<RequestItem>(this.StringToHeaderItems(this._HeaderText));
					foreach(RequestItem item in this._HeaderItems)
						item.PropertyChanged += HeaderItem_PropertyChanged;
					this._HeaderItems.CollectionChanged += HeaderItems_CollectionChanged;
				}
				return this._HeaderItems;
			}
			set
			{
				this._HeaderItems = value;
				foreach(RequestItem item in this._HeaderItems)
					item.PropertyChanged += HeaderItem_PropertyChanged;
				this._HeaderItems.CollectionChanged += HeaderItems_CollectionChanged;
			}
		}

		private void HeaderItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			this._HeaderText = null;
			this._ContentType = null;
			//this.NotifyOfPropertyChange(() => this.HeaderText);
			if(e.NewItems!=null)
				foreach (RequestItem item in e.NewItems)
					item.PropertyChanged += HeaderItem_PropertyChanged;
			//if (!this.HeaderCookieItem.ShowInEditor)
			//	this.HeaderItems.Move(this.HeaderItems.IndexOf(this.HeaderCookieItem), this.HeaderItems.Count - 1);

			if(e.Action==System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
				if(e.OldItems!=null)
				{
					if(e.OldItems.Cast<RequestItem>().Any(i=>i.Type==RequestItemType.Cookie))
					{
						this._HeaderCookieItem = null;
						if (this.HeaderItems.All(i => i.Type != RequestItemType.Cookie))
							this._CookieItems = null;
					}

					if (e.OldItems.Cast<RequestItem>().Any(i => i.Type == RequestItemType.ContentType))
						this._HeaderContentTypeItem = null;
				}
		}

		private void HeaderItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			this._HeaderText = null;
			this._ContentType = null;
			//this.NotifyOfPropertyChange(() => this.HeaderText);
			RequestItem item = (RequestItem)sender;
			if (item.Type==RequestItemType.Cookie)
			{
				if (this.HeaderCookieItem != item)
				{
					if (this._CookieItems != null)
						this.ReplaceSelectedOldItems(this._CookieItems, this.StringToCookieItems(item.Value).ToArray());
					this.HeaderItems.Remove(item);
					this.HeaderCookieItem.SetValueWithoutNotify(item.Value);
				}
				if (!this.HeaderItems.Contains(this.HeaderCookieItem))
					this.HeaderItems.Add(this.HeaderCookieItem);
				if (this._CookieItems != null)
					this.ReplaceSelectedOldItems(this._CookieItems, this.StringToCookieItems(this.HeaderCookieItem.Value).ToArray());
			}
			else
			{
				if (this._HeaderCookieItem == item)
				{
					this._HeaderCookieItem = null;
					//this._CookieItems = null;
					if (this._CookieItems != null)
						foreach (Models.RequestItem cookieItem in this._CookieItems)
							cookieItem.Selected = false;
				}
			}
			if (item.Type == RequestItemType.ContentType)
				this.NotifyOfPropertyChange(() => this.ContentType);
		}

		//public void AddHeaderItem()
		//{
		//	if (this.HeaderCookieItem.ShowInEditor)
		//		this.HeaderItems.Add(new RequestItem());
		//	else this.HeaderItems.Insert(this.HeaderItems.IndexOf(this.HeaderCookieItem), new RequestItem());
		//}


		private string _HeaderText;
		public string HeaderText
		{
			get
			{
				if(this._HeaderText==null)
				{
					if (this._HeaderItems == null)
						this._HeaderText = "";
					else this._HeaderText = string.Join("\r\n", this.HeaderItems.Where(i => i.ShowInRaw).Select(i => $"{i.Name}: {i.Value}"));
				}
				return this._HeaderText;
			}
			set
			{
				this._HeaderText = value;
				if (this._HeaderItems == null)
					this._HeaderItems = new System.Collections.ObjectModel.ObservableCollection<RequestItem>(this.StringToHeaderItems(value));
				else
				{
					RequestItem[] newItems = this.StringToHeaderItems(value).ToArray();
					this.ReplaceSelectedOldItems(this._HeaderItems, newItems);

					foreach (RequestItem newItem in newItems)
					{
						if (newItem.Type == RequestItemType.Cookie)
							if (this._CookieItems != null)
								this.ReplaceSelectedOldItems(this._CookieItems, this.StringToCookieItems(newItem.Value).ToArray());
						if (newItem.Type == RequestItemType.ContentType)
							this._HeaderContentTypeItem = null;
					}
				}
				this._ContentType = null;
			}
		}

		public void ReplaceSelectedOldItems(IList<RequestItem>oldItems, IList<RequestItem> newItems)
		{
			List<RequestItem> addItems = new List<RequestItem>();
			foreach (RequestItem newItem in newItems)
			{
				RequestItem oldItem = oldItems.FirstOrDefault(i => i.Name == newItem.Name);
				if (oldItem == null)
					addItems.Add(newItem);
				else
				{
					oldItem.Selected = true;
					oldItem.Value = newItem.Value;
				}
			}
			for (int i = oldItems.Count - 1; i >= 0; i--)
			{
				RequestItem oldItem = oldItems[i];
				if (oldItem.Selected)
					if (newItems.All(newItem => newItem.Name != oldItems[i].Name))
						oldItems[i].Selected = false;
						//oldItems.RemoveAt(i);
			}
			foreach (RequestItem item in addItems)
				oldItems.Add(item);
		}

		protected System.Collections.Generic.IEnumerable<RequestItem>StringToHeaderItems(string s)
		{
			return this.StringToRequestItems(s, new char[] { '\r', '\n' }, ':');
		}

		protected System.Collections.Generic.IEnumerable<RequestItem>StringToRequestItems(string s, char[] itemSeparator,char vluaeSeparator)
		{
			foreach(string line in s.Split(itemSeparator, StringSplitOptions.RemoveEmptyEntries))
			{
				string[] parameters = line.Split(new char[] { vluaeSeparator },2);
				RequestItem item = new RequestItem();
				item.Name = parameters[0].Trim();
				if (parameters.Length > 1)
					item.Value = parameters[1].Trim();
				if (item.Name == "" && item.Value == "")
					continue;
				yield return item;
			}
		}

		protected System.Collections.Generic.IEnumerable<RequestItem>StringToRequestItems(string s, char itemSeparator,char vluaeSeparator)
		{
			return this.StringToRequestItems(s, new char[] { itemSeparator }, vluaeSeparator);
		}

		private RequestItem _HeaderCookieItem;
		public RequestItem HeaderCookieItem
		{
			get
			{
				if(this._HeaderCookieItem==null)
				{
					this._HeaderCookieItem = this.HeaderItems.FirstOrDefault(i => i.Type==RequestItemType.Cookie);
					if (this._HeaderCookieItem == null)
					{
						this._HeaderCookieItem = new RequestItem("Cookie","");
						//this.HeaderItems.Add(this._HeaderCookieItem);
					}
					//this._HeaderCookieItem.PropertyChanged += HeaderCookieItem_PropertyChanged;
				}
				return this._HeaderCookieItem;
			}
		}

		private RequestItem _HeaderContentTypeItem;
		public RequestItem HeaderContentTypeItem
		{
			get
			{
				if(this._HeaderContentTypeItem==null)
				{
					this._HeaderContentTypeItem = this.HeaderItems.FirstOrDefault(i => i.Type==RequestItemType.ContentType);
					if (this._HeaderContentTypeItem == null)
					{
						this._HeaderContentTypeItem = new RequestItem("Content-Type","");
						this._HeaderContentTypeItem.PropertyChanged += HeaderContentTypeItem_PropertyChanged;
					}
				}
				return this._HeaderContentTypeItem;
			}
		}

		private void HeaderContentTypeItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (this._HeaderContentTypeItem != null)
				if (string.IsNullOrWhiteSpace(this._HeaderContentTypeItem.Value))
					this.HeaderItems.Remove(this._HeaderContentTypeItem);
				else if (!this._HeaderItems.Contains(this._HeaderContentTypeItem))
					this.HeaderItems.Add(this._HeaderContentTypeItem);

			this.NotifyOfPropertyChange(() => this.ContentType);
		}

		//private void HeaderCookieItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		//{

		//	if (this.HeaderCookieItem.IsCookieItem)
		//	{
		//		if (!this.HeaderItems.Contains(this.HeaderCookieItem))
		//			this.HeaderItems.Add(this.HeaderCookieItem);
		//		if (this._CookieItems != null)
		//			this.ReplaceSelectedOldItems(this._CookieItems, this.StringToCookieItems(this.HeaderCookieItem.Value).ToArray());
		//	}
		//	else
		//	{
		//		int index = this.HeaderItems.IndexOf(this.HeaderCookieItem);
		//		if (index >= 0)
		//			this.HeaderItems.RemoveAt(index);
		//		this._HeaderCookieItem = null;
		//		this._CookieItems = null;
		//	}
		//}

		public string Content_Type
		{
			get
			{
				return this.HeaderContentTypeItem.Value.Split(new char[] { ';' }, 2)[0].ToLower();
			}
		}

		public string CharSet
		{
			get
			{
				return this.HeaderContentTypeItem.Value.Split(new char[] { ';' }, 2).ElementAtOrDefault(1)?.Split('=')?.ElementAtOrDefault(1);
			}
		}

		private RequestContentType? _ContentType;
		public RequestContentType ContentType
		{
			get
			{
				if(this._ContentType==null)
					if (string.IsNullOrWhiteSpace(this.HeaderContentTypeItem.Value))
						this._ContentType = RequestContentType.PlainText;
					else switch(this.Content_Type)
					{
						case "application/x-www-form-urlencoded":
							this._ContentType = RequestContentType.FormUrlencoded;
							break;
						case "application/json":
							this._ContentType = RequestContentType.Json;
							break;
						case "application/xml":
							this._ContentType = RequestContentType.XML;
							break;
						default:
							this._ContentType = RequestContentType.PlainText;
							break;
					}
				return this._ContentType.Value;
			}
		}

		private System.Collections.ObjectModel.ObservableCollection<RequestItem> _CookieItems;
		[Newtonsoft.Json.JsonProperty(ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Replace)]
		public System.Collections.ObjectModel.ObservableCollection<RequestItem> CookieItems
		{
			get
			{
				if (this._CookieItems == null)
				{
					this._CookieItems = new System.Collections.ObjectModel.ObservableCollection<RequestItem>(this.StringToCookieItems(this.HeaderCookieItem.Value));
					foreach(RequestItem item in this._CookieItems)
						item.PropertyChanged += CookieItem_PropertyChanged;
					this._CookieItems.CollectionChanged += CookieItems_CollectionChanged;
				}
				return this._CookieItems;
			}
			set
			{
				this._CookieItems = value;
				foreach(RequestItem item in this._CookieItems)
					item.PropertyChanged += CookieItem_PropertyChanged;
				this._CookieItems.CollectionChanged += CookieItems_CollectionChanged;
			}
		}

		private void CookieItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null)
				foreach (RequestItem item in e.NewItems)
					item.PropertyChanged += CookieItem_PropertyChanged;
		}

		private void CookieItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			this.HeaderCookieItem.SetValueWithoutNotify(string.Join(";", this.CookieItems.Where(i => i.Selected).Select(i => $"{i.Name}={i.Value}")));
			if(string.IsNullOrWhiteSpace(this.HeaderCookieItem.Value))
			{
				int index = this.HeaderItems.IndexOf(this.HeaderCookieItem);
				if (index >= 0)
					this.HeaderItems.RemoveAt(index);
			}
			else
			{
				if (!this.HeaderItems.Contains(this.HeaderCookieItem))
					this.HeaderItems.Add(this.HeaderCookieItem);
			}
			//this.HeaderCookieItem.Value=string.Join(";", this.CookieItems.Where(i => i.Selected).Select(i => $"{i.Key}={i.Value}"));
			this._HeaderText = null;
			//this.NotifyOfPropertyChange(() => this.HeaderText);
			//this.HeaderCookieItem.NotifyOfPropertyChange(()=>this.HeaderCookieItem.ShowInEditor);
		}

		protected System.Collections.Generic.IEnumerable<RequestItem>StringToCookieItems(string s)
		{
			return this.StringToRequestItems(s, ';', '=');
		}

		private System.Net.Http.HttpMethod _HttpMethod;
		[Newtonsoft.Json.JsonProperty]
		[Newtonsoft.Json.JsonConverter(typeof(Converters.HttpMethodConverter))]
		public System.Net.Http.HttpMethod HttpMethod
		{
			get
			{
				if (this._HttpMethod == null)
					this._HttpMethod = System.Net.Http.HttpMethod.Get;
				return this._HttpMethod;
			}
			set
			{
				this._HttpMethod = value;
				bool acceptsRequestBody = this.AcceptsRequestBody;
				if (this._HeaderItems != null)
					foreach (RequestItem item in this.HeaderItems)
						if (item.Type == RequestItemType.ContentType)
							item.Selected = acceptsRequestBody;
				this.NotifyOfPropertyChange(() => this.HttpMethod);
			}
		}

		private string _FormRaw;
		public string FormRaw
		{
			get
			{
				if(this._FormRaw==null)
					this._FormRaw = string.Join("&", this.FormParameters.Where(p => p.Selected).Select(i => $"{i.Name}={System.Net.WebUtility.UrlEncode(i.Value)}"));
				return this._FormRaw;
			}
			set
			{
				this._FormRaw = value;
				if(this._FormParameters!=null)
					this.ReplaceSelectedOldItems(this._FormParameters, this.StringToUrlParameters(this._FormRaw).ToList());
			}
		}


		private System.Collections.ObjectModel.ObservableCollection<RequestItem> _FormParameters;
		[Newtonsoft.Json.JsonProperty(ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Replace)]
		public System.Collections.ObjectModel.ObservableCollection<RequestItem> FormParameters
		{
			get
			{
				if (this._FormParameters == null)
				{
					if (this._FormRaw == null)
						this._FormParameters = new System.Collections.ObjectModel.ObservableCollection<RequestItem>();
					else
					{
						this._FormParameters = new System.Collections.ObjectModel.ObservableCollection<RequestItem>(this.StringToUrlParameters(this._FormRaw));
						foreach (RequestItem item in this._FormParameters)
							item.PropertyChanged += FormParameter_PropertyChanged;
					}
					if(this._FormParameters.Count==0)
						if(!string.IsNullOrWhiteSpace(this._JsonContent))
						{
							try
							{
								this._FormParameters = new System.Collections.ObjectModel.ObservableCollection<RequestItem>(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(this._JsonContent).Select(i => new RequestItem(i.Key, i.Value)));
							}
							catch
							{
							}
						}
					if(this._FormParameters.Count==0)
						if(!string.IsNullOrWhiteSpace(this._XmlContent))
						{
							try
							{
								this._FormParameters = new System.Collections.ObjectModel.ObservableCollection<RequestItem>(System.Xml.Linq.XElement.Parse(this._XmlContent).Elements().Select(i => new RequestItem(i.Name.ToString(), i.Value)));
							}
							catch
							{
							}
						}
					this._FormParameters.CollectionChanged += FormParameters_CollectionChanged;
				}
				return this._FormParameters;
			}
			set
			{
				this._FormParameters = value;
				foreach (RequestItem item in this._FormParameters)
					item.PropertyChanged += FormParameter_PropertyChanged;
				this._FormParameters.CollectionChanged += FormParameters_CollectionChanged;
			}
		}

		private string _JsonContent;
		public string JsonContent
		{
			get
			{
				if(this._JsonContent==null)
				{
					if(this._FormParameters!=null)
					{
						RequestItem[] formParameters = this.FormParameters.Where(p => p.Selected).ToArray();
						if (formParameters.Length > 0)
							this._JsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(formParameters.ToDictionary(p => p.Name, p => p.Value));
					}
					if(this._JsonContent==null&&!string.IsNullOrWhiteSpace(this._XmlContent))
					{
						System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
						try
						{
							xmlDocument.LoadXml(this._XmlContent);
							this._JsonContent = Newtonsoft.Json.JsonConvert.SerializeXmlNode(xmlDocument.DocumentElement);
						}
						catch
						{
						}
					}
				}
				return this._JsonContent;
			}
			set
			{
				if (string.IsNullOrWhiteSpace(value))
					this._JsonContent = null;
				else
				{
					this._JsonContent = value;
					if (string.IsNullOrWhiteSpace(this._FormRaw) && this._FormParameters?.Count == 0)
						this._FormParameters = null;
					if (string.IsNullOrWhiteSpace(this._XmlContent))
						this._XmlContent = null;
				}
			}
		}

		private string _XmlContent;
		public string XmlContent
		{
			get
			{
				if(this._XmlContent==null)
				{
					if (this._FormParameters != null)
					{
						RequestItem[] formParameters = this.FormParameters.Where(p => p.Selected).ToArray();
						if (formParameters.Length > 0)
							this._XmlContent = this.XmlDocumentToString(Newtonsoft.Json.JsonConvert.DeserializeXmlNode(Newtonsoft.Json.JsonConvert.SerializeObject(new { root = formParameters.ToDictionary(p => p.Name, p => p.Value) })));
					}
					if (this._XmlContent == null && !string.IsNullOrWhiteSpace(this._JsonContent))
					{
						try
						{
							this._XmlContent = this.XmlDocumentToString(Newtonsoft.Json.JsonConvert.DeserializeXmlNode($"root:{{{this._JsonContent}}}"));
						}
						catch
						{
						}
					}
				}
				return this._XmlContent;
			}
			set
			{
				if (string.IsNullOrWhiteSpace(value))
					this._XmlContent = value;
				else
				{
					this._XmlContent = value;
					if (string.IsNullOrWhiteSpace(this._FormRaw) && this._FormParameters?.Count == 0)
						this._FormParameters = null;
					if (string.IsNullOrWhiteSpace(this._JsonContent))
						this._JsonContent = null;
				}
			}
		}

		public string PlainTextContent { get; set; }

		private void FormParameters_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null)
				foreach (RequestItem item in e.NewItems)
					item.PropertyChanged += FormParameter_PropertyChanged;
			this._FormRaw = null;
		}

		private void FormParameter_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			this._FormRaw = null;
		}

		protected string XmlDocumentToString(System.Xml.XmlDocument document)
		{
			System.IO.StringWriter writer = new System.IO.StringWriter();
			document.Save(writer);
			try
			{
				return writer.ToString();
			}
			finally
			{
				writer.Close();
			}
		}

		public bool AcceptsRequestBody
		{
			get
			{
				return this.HttpMethod == System.Net.Http.HttpMethod.Post || this.HttpMethod == System.Net.Http.HttpMethod.Put;
			}
		}

		private TaskStatus _Status;
		public TaskStatus Status
		{
			get
			{
				return this._Status;
			}
			set
			{
				this._Status = value;
				this.NotifyOfPropertyChange(() => this.Status);
			}
		}


		private Models.DataErrorInfos.Task _DataErrorInfo;
		public Models.DataErrorInfos.Task DataErrorInfo
		{
			get
			{
				return this._DataErrorInfo;
			}
			protected set
			{
				this._DataErrorInfo = value;
				this.NotifyOfPropertyChange(() => this.DataErrorInfo);
			}
		}

		public event System.Action Done;
		protected void OnDone()
		{
			if (this.Done != null)
				this.Done();
		}

		protected async Task<byte[]> StreamToBytes(System.IO.Stream stream)
		{
			System.IO.MemoryStream memory = new System.IO.MemoryStream();
			await stream.CopyToAsync(memory);
			memory.Position = 0;
			return memory.ToArray();
		}

		private System.Net.CookieContainer _CookieContainer;
		public System.Net.CookieContainer CookieContainer
		{
			get
			{
				if(this._CookieContainer==null)
				{
					this._CookieContainer = new System.Net.CookieContainer();
					if(this.HeaderCookieItem.Selected)
					foreach(RequestItem item in this.CookieItems)
						if(item.Selected)
							this._CookieContainer.Add(new System.Net.Cookie(item.Name, item.Value) { Domain = this.Uri.Host });
				}
				return this._CookieContainer;
			}
		}

		private System.Net.Http.HttpClient _HttpClient;
		public System.Net.Http.HttpClient HttpClient
		{
			get
			{
				if(this._HttpClient==null)
				{

					this._HttpClient = new System.Net.Http.HttpClient(new System.Net.Http.HttpClientHandler
					{
						CookieContainer = this.CookieContainer,
						UseCookies = true,
						//如果设置自动解压，会导致Headers里面丢失Content-Encoding
						//AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate,
						ServerCertificateCustomValidationCallback = (a, b, c, d) => true
					});

				}
				return this._HttpClient;
			}
		}

		public string PostStringContent
		{
			get
			{
				switch (this.ContentType)
				{
					case RequestContentType.FormUrlencoded:
						return this.FormRaw;
					case RequestContentType.Json:
						return this.JsonContent;
					case RequestContentType.XML:
						return this.XmlContent;
				}
				return this.PlainTextContent;
			}
		}

		public async System.Threading.Tasks.Task<Response> Run()
		{
			this._CookieContainer = null;
			this._HttpClient = null;

			System.Net.Http.HttpRequestMessage request = new System.Net.Http.HttpRequestMessage(this.HttpMethod, this.Url);
			foreach(RequestItem item in this.HeaderItems)
			{
				if (!item.Selected)
					continue;
				switch(item.Type)
				{
					case RequestItemType.Cookie:
						continue;
					case RequestItemType.ContentType:
						continue;
					default:
						request.Headers.Add(item.Name, item.Value);
						continue;
				}
			}
			if (this.AcceptsRequestBody)
				if (this.ContentType == RequestContentType.FormUrlencoded)
					request.Content = new System.Net.Http.FormUrlEncodedContent(this.FormParameters.Where(p => p.Selected).ToDictionary(p => p.Name, p => p.Value));
				else request.Content = new System.Net.Http.StringContent(this.PostStringContent);
			if(request.Content!=null)
			{
				string contentType = this.Content_Type;
				if (string.IsNullOrWhiteSpace(contentType))
					request.Content.Headers.ContentType = null;
				else
				{
					request.Content.Headers.ContentType.MediaType = this.Content_Type;
					request.Content.Headers.ContentType.CharSet = this.CharSet;
				}
			}


			this.Status = TaskStatus.Performing; 
			System.Net.Http.HttpResponseMessage response = await this.HttpClient.SendAsync(request);
			string headers = response.ToString();
			headers = headers.Substring(headers.IndexOf("{") + 1);
			headers = headers.Substring(0, headers.LastIndexOf("}"));
			headers = string.Join("\r\n",headers.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(h=>h.Trim()));
			string contentEncoding = response.Content.Headers.ContentEncoding.FirstOrDefault()?.ToLower();
			byte[] content;
			try
			{
				if(new string[] { "br", "brotli" }.Contains(contentEncoding))
					content = await this.StreamToBytes(new BrotliSharpLib.BrotliStream(await response.Content.ReadAsStreamAsync(), CompressionMode.Decompress));
				else if (contentEncoding == "gzip")
					content = await this.StreamToBytes(new System.IO.Compression.GZipStream(await response.Content.ReadAsStreamAsync(), CompressionMode.Decompress));
				else if (contentEncoding == "deflate")
					content = await this.StreamToBytes(new System.IO.Compression.DeflateStream(await response.Content.ReadAsStreamAsync(), CompressionMode.Decompress));
				else content = await response.Content.ReadAsByteArrayAsync();

				string fileName;
				try
				{
					fileName = response.Content.Headers.ContentDisposition?.FileName;
				}
				catch
				{
					fileName = null;
				}

				return new Response(headers, content, response.Content.Headers.ContentType?.MediaType, response.Content.Headers.ContentType?.CharSet, fileName);
			}
			finally
			{
				//var myTest = response.Headers.FirstOrDefault(h => h.Key == "Content-Encoding");
				this.Status = TaskStatus.Done;
			}
		}

		public void CancelPendingRequests()
		{
			this.HttpClient?.CancelPendingRequests();
		}

		public void Save(string path)
		{
			System.IO.StreamWriter writer = new System.IO.StreamWriter(path);
			//NetJSON.NetJSON.Serialize(this, stream);
			new Newtonsoft.Json.JsonSerializer().Serialize(writer, this);
			writer.Close();
			Setting.Instance.AddRecentFile(path);
		}

		public static Task Open(string path)
		{
			System.IO.StreamReader stream = new System.IO.StreamReader(path);
			try
			{
				//return NetJSON.NetJSON.Deserialize<Task>(stream);
				return new Newtonsoft.Json.JsonSerializer().Deserialize<Task>(new Newtonsoft.Json.JsonTextReader(stream));
			}
			finally
			{
				stream.Close();
				Setting.Instance.AddRecentFile(path);
			}
		}

		private static System.Text.RegularExpressions.Regex _UrlRegex;
		private System.Text.RegularExpressions.Regex UrlRegex
		{
			get
			{
				if (_UrlRegex == null)
					_UrlRegex = new System.Text.RegularExpressions.Regex(@"^https?://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$");
				return _UrlRegex;
			}
		}

		public bool ValidateData(Localization localization)
		{
			this.DataErrorInfo = new DataErrorInfos.Task();
			if (string.IsNullOrWhiteSpace(this.Url))
				this.DataErrorInfo.Url = string.Format(localization.ValidationError[ValidationError.Required],localization.Url);
			else if (!this.UrlRegex.IsMatch(this.Url))
				this.DataErrorInfo.Url = string.Format(localization.ValidationError[ValidationError.InvalidFormat],localization.Url);

			//bool result = true;

			return this.DataErrorInfo.Url == null;
		}
	}
}