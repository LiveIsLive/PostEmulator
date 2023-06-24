using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.PostEmulator.ViewModels
{
	public class Main : Screen, GongSolutions.Wpf.DragDrop.IDropTarget
	{
		private Models.Task _Task = new Models.Task();
		public Models.Task Task
		{
			get
			{
				return this._Task;
			}
			set
			{
				this._Task = value;
				this.NotifyOfPropertyChange(() => this.Task);
			}
		}

		public Models.Theme[] Themes { get; } = Models.Theme.All;

		private System.Globalization.CultureInfo[] _InstalledCultures;
		public System.Globalization.CultureInfo[] InstalledCultures
		{
			get
			{
				if (this._InstalledCultures == null)
				{
					System.Collections.Generic.List<System.Globalization.CultureInfo> cultures = new List<System.Globalization.CultureInfo>();
					foreach (string path in System.IO.Directory.GetFiles(LocalizationDirectory))
					{
						string name = System.IO.Path.GetFileNameWithoutExtension(path);
						try
						{
							cultures.Add(System.Globalization.CultureInfo.GetCultureInfo(name));
						}
						catch
						{
						}
					}
					this._InstalledCultures = cultures.ToArray();

				}
				return this._InstalledCultures;
			}
		}

		public static string _OpeningFilePath;
		public string OpeningFilePath
		{
			get
			{
				return _OpeningFilePath;
			}
			set
			{
				_OpeningFilePath = value;
				this.NotifyOfPropertyChange(() => this.OpeningFilePath);
			}
		}

		public static void SetOpeningFilePath(string path)
		{
			_OpeningFilePath = path;
		}

		private System.Collections.ObjectModel.ObservableCollection<string> _RecentFiles;
		public System.Collections.ObjectModel.ObservableCollection<string> RecentFiles
		{
			get
			{
				if (this._RecentFiles == null)
					this._RecentFiles = new System.Collections.ObjectModel.ObservableCollection<string>(this.Setting.RecentFiles.Where(f => f != OpeningFilePath));
				return this._RecentFiles;
			}
		}

		protected const string ProgramName = "Post Emulator";

		private bool _UpdateTabBinding;
		public bool UpdateTabBinding
		{
			get
			{
				return this._UpdateTabBinding;
			}
			set
			{
				this._UpdateTabBinding = value;
				this.NotifyOfPropertyChange(() => this.UpdateTabBinding);
			}
		}

		private string _Title = ProgramName;
		public string Title
		{
			get
			{
				return this._Title;
			}
			set
			{
				this._Title = value;
				this.NotifyOfPropertyChange(() => this.Title);
			}
		}

		private System.Net.Http.HttpMethod[] _HttpMethods;
		public System.Net.Http.HttpMethod[] HttpMethods
		{
			get
			{
				if(this._HttpMethods==null)
				{
					System.Collections.ObjectModel.ObservableCollection<System.Net.Http.HttpMethod> methods = new System.Collections.ObjectModel.ObservableCollection<System.Net.Http.HttpMethod>();
					foreach (System.Reflection.PropertyInfo property in typeof(System.Net.Http.HttpMethod).GetProperties(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public))
						methods.Add((System.Net.Http.HttpMethod)property.GetValue(null));
					methods.Move(methods.IndexOf(System.Net.Http.HttpMethod.Post), 1);
					this._HttpMethods = methods.ToArray();
				}
				return this._HttpMethods;
			}
		}

		public string[] CommonHeaderNames { get; } = new string[] { "Accept", "Accept-Charset", "Accept-Encoding", "Accept-Language", "Accept-Ranges", "Authorization", "Cache-Control", "Connection", "Cookie", "Content-Type", "Date", "Expect", "From", "Host", "If-Match", "If-Modified-Since", "If-None-Match", "If-Range", "If-Unmodified-Since", "Max-Forwards", "Pragma", "Proxy-Authorization", "Range", "Referer", "TE", "Upgrade", "User-Agent", "Via", "Warning" };

		public string[] CommonContentTypes { get; } = new string[] { "application/x-www-form-urlencoded", "application/json", "application/xml", "multipart/form-data" };

		public Models.Codes.BaseTemplate[] CodeTemplates { get; } = new Models.Codes.BaseTemplate[] { new Models.Codes.CSharp(), new Models.Codes.Java(), new Models.Codes.Python(), new Models.Codes.JavaScript() };

		public void ExportCode(Models.Codes.BaseTemplate template)
		{
			this.UpdateTabBinding = true;
			template.Task = this.Task;
			this.WindowManager.ShowDialogAsync(new Code(template));
		}

		public void Save(string path)
		{
			this.UpdateTabBinding = true;
			if (this.OpeningFilePath == null)
				this.OpeningFilePath = path;
			this.Save();
		}

		public void SaveAs(string path)
		{
			this.UpdateTabBinding = true;
			this.OpeningFilePath = path;
			this.Save();
		}

		public void Save()
		{
			this.Task.Save(OpeningFilePath);
			this._RecentFiles = null;
			this.NotifyOfPropertyChange(() => this.RecentFiles);
			this.SetTitle();
		}

		public void Open(string path)
		{
			this.Task = Models.Task.Open(path);
			this.OpeningFilePath = path;
			this._RecentFiles = null;
			this.NotifyOfPropertyChange(() => this.RecentFiles);
			this.SetTitle();
		}

		public void RemoveRecentFile(string path)
		{
			this.RecentFiles.Remove(path);
			this.Setting.RecentFiles.Remove(path);
			this.Setting.Save();
		}

		protected void SetTitle()
		{
			this.Title = ProgramName + " - " + System.IO.Path.GetFileNameWithoutExtension(OpeningFilePath);
		}


		protected Models.Localization GetLocalization(System.Globalization.CultureInfo culture)
		{
			System.IO.StreamReader reader = new System.IO.StreamReader(LocalizationDirectory + culture.Name + ".json");
			try
			{
				return new Newtonsoft.Json.JsonSerializer().Deserialize<Models.Localization>(new Newtonsoft.Json.JsonTextReader(reader));
				//return NetJSON.NetJSON.Deserialize<Models.Localization>(reader);
			}
			finally
			{
				reader.Close();
			}
		}

		public void SelectLanguage(System.Globalization.CultureInfo culture)
		{
			System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
			System.Threading.Thread.CurrentThread.CurrentCulture = culture;

			this.Localization = this.GetLocalization(culture);
			Models.Global.Instance.Localization = this.Localization;
			this.Setting.SelectedCultureName = culture.Name;
			//this.SetUiLang(this.Setting.SelectedCultureName);
			System.Threading.Tasks.Task.Run(() => this.Setting.Save());
		}

		private string _ConfirmMessage;
		public string ConfirmMessage
		{
			get
			{
				return this._ConfirmMessage;
			}
			set
			{
				this._ConfirmMessage = value;
				this.NotifyOfPropertyChange(() => this.ConfirmMessage);
			}
		}

		private bool _ShowConfirmMessage;
		public bool ShowConfirmMessage
		{
			get
			{
				return this._ShowConfirmMessage;
			}
			set
			{
				this._ShowConfirmMessage = value;
				this.NotifyOfPropertyChange(() => this.ShowConfirmMessage);
			}
		}

		public bool ConfirmResult { get; set; }

		public void Run()
		{
			this.UpdateTabBinding = true;

			if (!this.ValidateData())
				return;

			//if (!this.Task.CompressToZipFile)
			//	foreach (Models.Job job in this.Task.Jobs)
			//		if (!job.ResultHandler.TargetDirectoryEmpty(job))
			//		{
			//			this.ConfirmMessage = String.Format(this.Localization.TargetDirectoryIsNotEmpty, job.TargetDirectoryPath);
			//			this.ShowConfirmMessage = true;
			//			if (!ConfirmResult)
			//				return;
			//		}
						//if (this.DialogService.ShowMessageBox(this, String.Format(this.Localization.TargetDirectoryIsNotEmpty, job.TargetDirectoryPath), "", System.Windows.MessageBoxButton.OKCancel, System.Windows.MessageBoxImage.Warning) != System.Windows.MessageBoxResult.OK)
						//	return;

			this.WindowManager.ShowDialogAsync(new Runner(this.Task, System.IO.Path.GetFileNameWithoutExtension(this.OpeningFilePath)));
		}

		public void UncheckedCompressToZipFile()
		{
		}

		public bool ValidateData()
		{
			return this.Task.ValidateData(this.Localization);
			//if (this.Task.ValidateData(this.Localization))
			//	return true;
			//if (this.Task.DataErrorInfo.LastInvalidJob != null)
			//	this.SelectedJob = this.Task.DataErrorInfo.LastInvalidJob;
			//return false;
		}

		//public void EnsureJobBinding()
		//{
		//	Models.Job job = this.SelectedJob;
		//	this.SelectedJob = null;
		//	this.SelectedJob = job;
		//}
		public void ShowTutorial()
		{
			System.Diagnostics.Process.Start("https://github.com/LiveIsLive/PostEmulator/");
		}

		public void ShowAboutWindow()
		{
			this.WindowManager.ShowDialogAsync(new About());
		}

		void IDropTarget.DragEnter(IDropInfo dropInfo)
		{
		}

		void IDropTarget.DragOver(IDropInfo dropInfo)
		{
			if (dropInfo.Data == null || dropInfo.TargetItem == null)
				return;

			dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
			dropInfo.Effects = System.Windows.DragDropEffects.Move;
		}

		void IDropTarget.DragLeave(IDropInfo dropInfo)
		{
		}

		void IDropTarget.Drop(IDropInfo dropInfo)
		{
			//Models.Condition sourceCondition = (Models.Condition)dropInfo.Data;
			//Models.Condition targetCondition = (Models.Condition)dropInfo.TargetItem;

			//if (sourceCondition == null || targetCondition == null)
			//	return;
			
			//int oldIndex=this.SelectedJob.Conditions.IndexOf(sourceCondition);
			//this.SelectedJob.Conditions.Move(oldIndex, dropInfo.InsertIndex);
		}

		public void ChangeTheme(Models.Theme theme)
		{
			if (theme.Parent == null)
				return;

			this.Setting.ThemeId = theme.ThemeId;
			this.Setting.Save();
		}

		public void SetListElementValue(System.Collections.IList list,int index, System.Windows.RoutedPropertyChangedEventArgs<string> args)
		{
			list[index] = args.NewValue;
		}

		public void MoveUpListElement(System.Collections.ObjectModel.ObservableCollection<string> list,int index)
		{
			list.Move(index, index - 1);
		}

		public void MoveDownListElement(System.Collections.ObjectModel.ObservableCollection<string> list,int index)
		{
			list.Move(index, index + 1);
		}

		public void RemoveListElement(System.Collections.IList list,int index)
		{
			list.RemoveAt(index);
		}






		public void AddHeader()
		{
			this.Task.HeaderItems.Add(new Models.RequestItem());
		}

		public void MoveUpHeader(Models.RequestItem item)
		{
			int index = this.Task.HeaderItems.IndexOf(item);
			if (index > 0)
				this.Task.HeaderItems.Move(index, index - 1);
		}

		public void MoveDownHeader(Models.RequestItem item)
		{
			int index = this.Task.HeaderItems.IndexOf(item);
			if (index < this.Task.HeaderItems.Count - 1)
				this.Task.HeaderItems.Move(index, index + 1);
		}

		public void RemoveHeader(Models.RequestItem item)
		{
			this.Task.HeaderItems.Remove(item);
		}


		public void AddCookie()
		{
			this.Task.CookieItems.Add(new Models.RequestItem());
		}

		public void MoveUpCookie(Models.RequestItem item)
		{
			int index = this.Task.CookieItems.IndexOf(item);
			if (index > 0)
				this.Task.CookieItems.Move(index, index - 1);
		}

		public void MoveDownCookie(Models.RequestItem item)
		{
			int index = this.Task.CookieItems.IndexOf(item);
			if (index < this.Task.CookieItems.Count - 1)
				this.Task.CookieItems.Move(index, index + 1);
		}

		public void RemoveCookie(Models.RequestItem item)
		{
			this.Task.CookieItems.Remove(item);
		}
	}
}