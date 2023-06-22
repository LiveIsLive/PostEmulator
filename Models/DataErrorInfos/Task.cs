using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.HttpClientPerformer.Models.DataErrorInfos
{
	public class Task : Models.Task
	{
		private string _Url;
		public override string Url
		{
			get
			{
				return this._Url;
			}
			set
			{
				this._Url = value;
				this.NotifyOfPropertyChange(() => this.Url);
			}
		}
	}
}