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
		[Newtonsoft.Json.JsonProperty]
		public string Url { get; set; }

		private System.Collections.ObjectModel.ObservableCollection<RequestItem> _HeaderItems;
		[Newtonsoft.Json.JsonProperty]
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
		}

		private void HeaderItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			this._HeaderText = null;
			this.NotifyOfPropertyChange(() => this.HeaderText);
			if(e.NewItems!=null)
				foreach (RequestItem item in e.NewItems)
					item.PropertyChanged += HeaderItem_PropertyChanged;
		}

		private void HeaderItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			this._HeaderText = null;
			this.NotifyOfPropertyChange(() => this.HeaderText);
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
					else this._HeaderText = string.Join("\r\n", this.HeaderItems.Where(i => i.ShowInRaw).Select(i => $"{i.Key}: {i.Value}"));
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
				RequestItem oldItem = oldItems.FirstOrDefault(i => i.Key == newItem.Key);
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
					if (newItems.All(newItem => newItem.Key != oldItems[i].Key))
						oldItems.RemoveAt(i);
			}
			foreach (RequestItem item in addItems)
				oldItems.Add(item);
		}

		protected System.Collections.Generic.IEnumerable<RequestItem>StringToHeaderItems(string s)
		{
			foreach(string line in s.Split(new char[] { '\r', '\n' },StringSplitOptions.RemoveEmptyEntries))
			{
				string[] parameters = line.Split(':');
				RequestItem item = new RequestItem();
				item.Key = parameters[0].Trim();
				if (parameters.Length > 1)
					item.Value = String.Join("", parameters.Skip(1).Select(p => p.Trim())).Trim();
				if (item.Key == "" && item.Value == "")
					continue;
				yield return item;
			}
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
						this.HeaderItems.Add(this._HeaderCookieItem);
					}
					this._HeaderCookieItem.PropertyChanged += HeaderCookieItem_PropertyChanged;
				}
				return this._HeaderCookieItem;
			}
		}

		private void HeaderCookieItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (this._CookieItems != null)
				this.ReplaceSelectedOldItems(this._CookieItems, this.StringToCookieItems(this.HeaderCookieItem.Value).ToArray());
		}

		private System.Collections.ObjectModel.ObservableCollection<RequestItem> _CookieItems;
		[Newtonsoft.Json.JsonProperty]
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
			this.HeaderCookieItem.SetValueWithoutNotify(string.Join(";", this.CookieItems.Where(i => i.Selected).Select(i => $"{i.Key}={i.Value}")));
			this._HeaderText = null;
			this.NotifyOfPropertyChange(() => this.HeaderText);
			//this.HeaderCookieItem.NotifyOfPropertyChange(()=>this.HeaderCookieItem.ShowInEditor);
		}

		protected System.Collections.Generic.IEnumerable<RequestItem>StringToCookieItems(string s)
		{
			foreach(string line in s.Split(new char[] { ';' },StringSplitOptions.RemoveEmptyEntries))
			{
				string[] parameters = line.Split('=');
				RequestItem item = new RequestItem();
				item.Key = parameters[0].Trim();
				if (parameters.Length > 1)
					item.Value = String.Join("", parameters.Skip(1).Select(p => p.Trim())).Trim();
				if (item.Key == "" && item.Value == "")
					continue;
				yield return item;
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

		public void Run()
		{
			this.Status = TaskStatus.Performing;
			//if (this.AutoRunWhenFilesFiltered)
			//	this.CopyFiles();
			//else this.Status = TaskStatus.Standby;
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
			bool result = true;

			return result;
		}
	}
}