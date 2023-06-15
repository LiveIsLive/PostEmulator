using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.HttpClientPerformer.Models
{
	public class RequestItem : Caliburn.Micro.PropertyChangedBase
	{
		private bool _Selected = true;
		[Newtonsoft.Json.JsonProperty]
		public bool Selected
		{
			get
			{
				return this._Selected;
			}
			set
			{
				this._Selected = value;
				this._ShowInRaw = null;
				this.NotifyOfPropertyChange(() => this.Selected);
				//this.NotifyOfPropertyChange(() => this.ShowInRaw);
			}
		}

		[Newtonsoft.Json.JsonProperty]
		private string _Key;
		public string Name
		{
			get
			{
				return this._Key;
			}
			set
			{
				this._Key = value;
				this._Type = null;
				this._ShowInRaw = null;
				//this._ShowInEditor = null;
				this.NotifyOfPropertyChange(() => this.Name);
				//this.NotifyOfPropertyChange(() => this.ShowInRaw);
				//this.NotifyOfPropertyChange(() => this.ShowInEditor);
			}
		}

		private string _Value;
		[Newtonsoft.Json.JsonProperty]
		public string Value
		{
			get
			{
				return this._Value;
			}
			set
			{
				this._Value = value;
				this._ShowInRaw = null;
				//this._ShowInEditor = null;
				this.NotifyOfPropertyChange(() => this.Value);
				//this.NotifyOfPropertyChange(() => this.ShowInRaw);
				//this.NotifyOfPropertyChange(() => this.ShowInEditor);
				//this.OnValueChanged();
			}
		}

		public void SetValueWithoutNotify(string value)
		{
			this._Value = value;
			this._ShowInRaw = null;
			//this._ShowInEditor = null;
		}

		//public event System.Action<string> ValueChanged;
		//protected void OnValueChanged()
		//{
		//	if (this.ValueChanged != null)
		//		this.ValueChanged(this.Value);
		//}

		//private bool? _IsCookieItem;
		//public bool IsCookieItem
		//{
		//	get
		//	{
		//		if (this._IsCookieItem == null)
		//			this._IsCookieItem = string.Equals(this.Name, "Cookie", StringComparison.OrdinalIgnoreCase);
		//		return this._IsCookieItem.Value;
		//	}
		//}

		private RequestItemType? _Type;
		public RequestItemType Type
		{
			get
			{
				if (this._Type == null)
					if (string.Equals(this.Name, "Cookie", StringComparison.OrdinalIgnoreCase))
						this._Type = RequestItemType.Cookie;
					else if (string.Equals(this.Name, "Content-Type", StringComparison.OrdinalIgnoreCase))
						this._Type = RequestItemType.ContentType;
					else this._Type = RequestItemType.Common;
				return this._Type.Value;
			}
		}

		private bool? _ShowInRaw;
		public bool ShowInRaw
		{
			get
			{
				if(this._ShowInRaw==null)
					if (this.Selected)
					{
						bool noValue = string.IsNullOrWhiteSpace(this.Value);
						if (this.Type == RequestItemType.Cookie && noValue)
							this._ShowInRaw = false;
						else if (string.IsNullOrWhiteSpace(this.Name) && noValue)
							this._ShowInRaw = false;
						else this._ShowInRaw = true;
					}
					else this._ShowInRaw = false;
				return this._ShowInRaw.Value;
			}
		}

		//private bool? _ShowInEditor;
		//public bool ShowInEditor
		//{
		//	get
		//	{
		//		if (this._ShowInEditor == null)
		//			if (this.IsCookieItem && string.IsNullOrWhiteSpace(this.Value))
		//				this._ShowInEditor = false;
		//			else this._ShowInEditor = true;
		//		return this._ShowInEditor.Value;
		//	}
		//}

		public RequestItem()
		{

		}

		public RequestItem(string name)
		{
			this._Key = name;
		}

		public RequestItem(string name, string value)
		{
			this._Key = name;
			this._Value = value;
		}
	}
}
