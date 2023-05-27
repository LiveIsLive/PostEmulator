using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.HttpClientPerformer.Models
{
	public class Global : Caliburn.Micro.PropertyChangedBase
	{
		private Localization _Localization;
		public Localization Localization
		{
			get
			{
				return this._Localization;
			}
			set
			{
				this._Localization = value;
				this.NotifyOfPropertyChange(() => this.Localization);
			}
		}

		public static readonly Global Instance = new Global();
	}
}