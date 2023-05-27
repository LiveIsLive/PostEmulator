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
					this._HeaderItems.CollectionChanged += HeaderItems_CollectionChanged;
				}
				return this.HeaderItems;
			}
		}

		private void HeaderItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
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
					else this._HeaderText = string.Join("\r\n", this.HeaderItems.Where(i => i.Selected).Select(i => $"{i.Name}: {i.Value}"));
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
					List<RequestItem> addItems = new List<RequestItem>();
					foreach(RequestItem newItem in newItems)
					{
						RequestItem oldItem = this._HeaderItems.FirstOrDefault(i => i.Name == newItem.Name);
						if (oldItem == null)
							addItems.Add(newItem);
						else
						{
							oldItem.Selected = true;
							oldItem.Value = newItem.Value;
						}
					}
					for(int i=this._HeaderItems.Count-1;i>=0;i--)
					{
						RequestItem oldItem = this._HeaderItems[i];
						if (oldItem.Selected)
							if (newItems.All(newItem => newItem.Name != this._HeaderItems[i].Name))
								this._HeaderItems.RemoveAt(i);
					}
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
				item.Name = parameters[0].Trim();
				if (parameters.Length > 1)
					item.Value = String.Join("", parameters.Skip(1).Select(p => p.Trim())).Trim();
				if (item.Name == "" && item.Value == "")
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
						this._HeaderCookieItem = new RequestItem();
						this.HeaderItems.Add(this._HeaderCookieItem);
					}
				}
				return this._HeaderCookieItem;
			}
		}

		private System.Collections.ObjectModel.ObservableCollection<RequestItem> _CookieItems;
		[Newtonsoft.Json.JsonProperty]
		public System.Collections.ObjectModel.ObservableCollection<RequestItem> CookieItems
		{
			get
			{
				if (this._CookieItems == null)
					this._CookieItems = new System.Collections.ObjectModel.ObservableCollection<RequestItem>(this.StringToCookieItems(this.HeaderCookieItem.Value));
				return this._CookieItems;
			}
			set
			{
				this._CookieItems = value;
			}
		}

		protected System.Collections.Generic.IEnumerable<RequestItem>StringToCookieItems(string s)
		{
			foreach(string line in s.Split(new char[] { ';' },StringSplitOptions.RemoveEmptyEntries))
			{
				string[] parameters = line.Split('=');
				RequestItem item = new RequestItem();
				item.Name = parameters[0].Trim();
				if (parameters.Length > 1)
					item.Value = String.Join("", parameters.Skip(1).Select(p => p.Trim())).Trim();
				if (item.Name == "" && item.Value == "")
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