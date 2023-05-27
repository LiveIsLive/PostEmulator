using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.HttpClientPerformer.ViewModels
{
	public class About : Screen
	{
		private static string _Version;
		public string Version
		{
			get
			{
				if (_Version == null)
					_Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
				return _Version;
			}
		}

		public int Year { get; } = System.DateTime.Now.Year;

		public void ShowHomePage()
		{
			System.Diagnostics.Process.Start("https://cold-shine-soft.com/");
		}
	}
}