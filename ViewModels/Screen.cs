using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.PostEmulator.ViewModels
{
	public class Screen : Caliburn.Micro.Screen
	{
		protected readonly System.Threading.CancellationTokenSource CancellationTokenSource = new System.Threading.CancellationTokenSource();

		protected static readonly string LocalizationDirectory = Models.Localization.InstallationDirectory;

		private Models.Setting _Setting;
		public Models.Setting Setting
		{
			get
			{
				if (this._Setting == null)
				{
					this._Setting = Models.Setting.Instance;
					//if(string.IsNullOrWhiteSpace(this._Setting.SelectedCultureName))
					//{
					//	string baseDirectory = LocalizationDirectory;
					//	string filePath = baseDirectory + System.Globalization.CultureInfo.CurrentUICulture.Name + ".json";
					//	if (System.IO.File.Exists(filePath))
					//		this._Setting.SelectedCultureName = System.Globalization.CultureInfo.CurrentUICulture.Name;
					//	else
					//	{
					//		filePath = baseDirectory + System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName + ".json";
					//		if (System.IO.File.Exists(filePath))
					//			this._Setting.SelectedCultureName = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
					//		else this._Setting.SelectedCultureName = "en";
					//	}
					//}
					//System.Globalization.CultureInfo culture = (System.Globalization.CultureInfo)System.Globalization.CultureInfo.GetCultureInfo(this._Setting.SelectedCultureName).Clone();

					//System.Windows.Application.Current.Dispatcher.Invoke(() =>
					//{
					//	System.Threading.Thread.CurrentThread.CurrentCulture= culture;
					//	System.Threading.Thread.CurrentThread.CurrentUICulture= culture;
					//});
					//this.SetUiLang(this.Setting.SelectedCultureName);
				}
				return this._Setting;
			}
		}

		private Models.Localization _Localization;
		public Models.Localization Localization
		{
			get
			{
				if (this._Localization == null)
					//if (Caliburn.Micro.Execute.InDesignMode)
					//{
					//	System.IO.StreamReader reader = new System.IO.StreamReader(System.Windows.Application.GetResourceStream(new Uri("/ColdShineSoft.PostEmulator.Views;component/Localization/zh-CN.json", System.UriKind.Relative)).Stream);
					//	this._Localization = NetJSON.NetJSON.Deserialize<Models.Localization>(reader);
					//	reader.Close();
					//}
					//else
					{
						System.IO.StreamReader reader = new System.IO.StreamReader(LocalizationDirectory + this.Setting.SelectedCulture.Name + ".json");
						//this._Localization = NetJSON.NetJSON.Deserialize<Models.Localization>(reader);
						this._Localization = new Newtonsoft.Json.JsonSerializer().Deserialize<Models.Localization>(new Newtonsoft.Json.JsonTextReader(reader));
					Models.Global.Instance.Localization = this._Localization;
						reader.Close();
					}
				return this._Localization;
			}
			set
			{
				this._Localization = value;
				this.NotifyOfPropertyChange(() => this.Localization);
			}
		}

		private Caliburn.Micro.IWindowManager _WindowManager;
		public Caliburn.Micro.IWindowManager WindowManager
		{
			get
			{
				if (this._WindowManager == null)
					this._WindowManager = new Caliburn.Micro.WindowManager();
				return this._WindowManager;
			}
		}

		//private MvvmDialogs.IDialogService _DialogService;
		//public MvvmDialogs.IDialogService DialogService
		//{
		//	get
		//	{
		//		if (this._DialogService == null)
		//			this._DialogService = new MvvmDialogs.DialogService();
		//		return this._DialogService;
		//	}
		//}

		//protected static readonly System.Type UiConfigHelperType = System.Type.GetType("HandyControl.Tools.ConfigHelper,HandyControl");

		//protected static readonly object UiConfigHelper = UiConfigHelperType.GetField("Instance").GetValue(null);

		//protected static System.Reflection.MethodInfo SetUiLangMethod = UiConfigHelperType.GetMethod("SetLang");

		//protected void SetUiLang(string name)
		//{
		//	try
		//	{
		//		SetUiLangMethod.Invoke(UiConfigHelper, new object[] { name });
		//	}
		//	catch
		//	{
		//		try
		//		{
		//			SetUiLangMethod.Invoke(UiConfigHelper, new object[] { name.Split('-')[0] });
		//		}
		//		catch
		//		{
		//			SetUiLangMethod.Invoke(UiConfigHelper, new object[] { "en" });
		//		}
		//	}
		//}

		//public void CloseWindow()
		//{
		//	this.TryCloseAsync();
		//}
	}
}
