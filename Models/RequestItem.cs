using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.HttpClientPerformer.Models
{
	public class RequestItem
	{
		public bool Selected { get; set; } = true;

		private string _Name;
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				this._Name = value;
				this._IsCookieItem = null;
			}
		}

		public string Value { get; set; }

		private bool? _IsCookieItem;
		public bool IsCookieItem
		{
			get
			{
				if (this._IsCookieItem == null)
					this._IsCookieItem = string.Equals(this.Name, "Cookie", StringComparison.OrdinalIgnoreCase);
				return this._IsCookieItem.Value;
			}
		}

		public RequestItem()
		{

		}

		public RequestItem(string name)
		{
			this._Name = name;
		}

		public RequestItem(string name, string value)
		{
			this._Name = name;
			this.Value = value;
		}
	}
}
