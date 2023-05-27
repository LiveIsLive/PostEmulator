using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ColdShineSoft.HttpClientPerformer.Views
{
	/// <summary>
	/// App.xaml 的交互逻辑
	/// </summary>
	public partial class App : Application
	{
		public App()
		{
			//HandyControl.Tools.ConfigHelper.Instance.SetNavigationWindowDefaultStyle();
			System.AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
		}

		private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			System.IO.File.AppendAllText(System.AppContext.BaseDirectory + "Error.txt", e.ExceptionObject.ToString());
		}
	}
}
