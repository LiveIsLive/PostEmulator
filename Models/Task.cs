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
		public string Url
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
		private System.Uri Uri
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

			this.NotifyOfPropertyChange(() => this.FirstUrlParameter);
			this.NotifyOfPropertyChange(() => this.LastUrlParameter);
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
			//this.NotifyOfPropertyChange(() => this.HeaderText);
			if(e.NewItems!=null)
				foreach (RequestItem item in e.NewItems)
					item.PropertyChanged += HeaderItem_PropertyChanged;
			//if (!this.HeaderCookieItem.ShowInEditor)
			//	this.HeaderItems.Move(this.HeaderItems.IndexOf(this.HeaderCookieItem), this.HeaderItems.Count - 1);

			if(e.Action==System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
				if(e.OldItems!=null)
					if(e.OldItems.Cast<RequestItem>().Any(i=>i.IsCookieItem))
					{
						this._HeaderCookieItem = null;
						if (this.HeaderItems.All(i => !i.IsCookieItem))
							this._CookieItems = null;
					}

			this.NotifyOfPropertyChange(() => this.FirstHeader);
			this.NotifyOfPropertyChange(() => this.LastHeader);
		}

		private void HeaderItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			this._HeaderText = null;
			//this.NotifyOfPropertyChange(() => this.HeaderText);
			RequestItem item = (RequestItem)sender;
			if (item.IsCookieItem)
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
					this._CookieItems = null;
				}
				//else
				//{
				//	int index = this.HeaderItems.IndexOf(this.HeaderCookieItem);
				//	if (index >= 0)
				//		this.HeaderItems.RemoveAt(index);
				//	this._HeaderCookieItem = null;
				//	this._CookieItems = null;
				//}
			}
		}

		//public void AddHeaderItem()
		//{
		//	if (this.HeaderCookieItem.ShowInEditor)
		//		this.HeaderItems.Add(new RequestItem());
		//	else this.HeaderItems.Insert(this.HeaderItems.IndexOf(this.HeaderCookieItem), new RequestItem());
		//}

		public RequestItem FirstHeader
		{
			get
			{
				return this.HeaderItems.FirstOrDefault();
			}
		}

		public RequestItem LastHeader
		{
			get
			{
				return this.HeaderItems.LastOrDefault();
			}
		}

		public RequestItem FirstCookie
		{
			get
			{
				return this.CookieItems.FirstOrDefault();
			}
		}

		public RequestItem LastCookie
		{
			get
			{
				return this.CookieItems.LastOrDefault();
			}
		}

		public RequestItem FirstUrlParameter
		{
			get
			{
				return this.UrlParameters.FirstOrDefault();
			}
		}

		public RequestItem LastUrlParameter
		{
			get
			{
				return this.UrlParameters.LastOrDefault();
			}
		}

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
						if (newItem.IsCookieItem)
							if (this._CookieItems != null)
								this.ReplaceSelectedOldItems(this._CookieItems, this.StringToCookieItems(newItem.Value).ToArray());
				}
			}
		}

		protected void ReplaceSelectedOldItems(IList<RequestItem>oldItems, IList<RequestItem> newItems)
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
					this._HeaderCookieItem = this.HeaderItems.FirstOrDefault(i => i.IsCookieItem);
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

			this.NotifyOfPropertyChange(() => this.FirstCookie);
			this.NotifyOfPropertyChange(() => this.LastCookie);
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

		public async System.Threading.Tasks.Task<Response> Run()
		{
			this._CookieContainer = null;
			this._HttpClient = null;

			System.Net.Http.HttpRequestMessage request = new System.Net.Http.HttpRequestMessage(this.HttpMethod, this.Url);
			foreach(RequestItem item in this.HeaderItems)
				if(item.Selected&&!item.IsCookieItem)
					request.Headers.Add(item.Name, item.Value);
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

		public bool ValidateData(Localization localization)
		{
			this.DataErrorInfo = new DataErrorInfos.Task();
			if (string.IsNullOrWhiteSpace(this.Url))
				this.DataErrorInfo.Url = "必须录入Url";
			bool result = true;

			return result;
		}
	}
}