using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.PostEmulator.Models
{
	public class Setting:Caliburn.Micro.PropertyChangedBase
	{
		private string _SelectedCultureName;
		[Newtonsoft.Json.JsonProperty]
		public string SelectedCultureName
		{
			get
			{
				return this._SelectedCultureName;
			}
			set
			{
				this._SelectedCultureName = value;
				this._SelectedCulture = null;
			}
		}

		private System.Globalization.CultureInfo _SelectedCulture;
		public System.Globalization.CultureInfo SelectedCulture
		{
			get
			{
				if(this._SelectedCulture==null)
				{
					string cultureName = this.SelectedCultureName;
					if (string.IsNullOrWhiteSpace(this.SelectedCultureName))
					{
						string baseDirectory = Localization.InstallationDirectory;
						string filePath = baseDirectory + System.Globalization.CultureInfo.CurrentUICulture.Name + ".json";
						if (System.IO.File.Exists(filePath))
							cultureName = System.Globalization.CultureInfo.CurrentUICulture.Name;
						else cultureName = "en";
					}
					this._SelectedCulture = (System.Globalization.CultureInfo)System.Globalization.CultureInfo.GetCultureInfo(cultureName).Clone();
				}
				return this._SelectedCulture;
			}
		}

		[Newtonsoft.Json.JsonProperty]
		public byte MaxRecentFileCount { get; set; } = 50;

		private int _ThemeId;
		[Newtonsoft.Json.JsonProperty]
		public int ThemeId
		{
			get
			{
				if (this._ThemeId == 0)
					this._ThemeId = Theme.Default.ThemeId;
				return this._ThemeId;
			}
			set
			{
				this._ThemeId = value;
				this._Theme = null;
				this.NotifyOfPropertyChange(() => this.Theme);
			}
		}

		private Theme _Theme;
		public Theme Theme
		{
			get
			{
				if (this._Theme == null)
					this._Theme = Theme.FromId(this.ThemeId);
				return this._Theme;
			}
		}

		private System.Collections.ObjectModel.ObservableCollection<string> _RecentFiles;
		[Newtonsoft.Json.JsonProperty]
		public System.Collections.ObjectModel.ObservableCollection<string> RecentFiles
		{
			get
			{
				if (this._RecentFiles == null)
					this._RecentFiles = new System.Collections.ObjectModel.ObservableCollection<string>();
				return this._RecentFiles;
			}
			set
			{
				this._RecentFiles = value;
			}
		}

		protected static readonly string SavePath = System.IO.Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Cold-Shine-Soft.Com" , "Post Emulator", "Setting.json");

		private static Setting _Instance;
		public static Setting Instance
		{
			get
			{
				if (_Instance == null)
					if (System.IO.File.Exists(SavePath))
					{
						System.IO.StreamReader reader = new System.IO.StreamReader(SavePath);
						//_Instance = NetJSON.NetJSON.Deserialize<Setting>(reader);
						_Instance = new Newtonsoft.Json.JsonSerializer().Deserialize<Setting>(new Newtonsoft.Json.JsonTextReader(reader));
						if (_Instance == null)
							_Instance = new Setting();
						reader.Close();
					}
					else _Instance = new Setting();
				return _Instance;
			}
		}

		public void AddRecentFile(string path)
		{
			int index = this.RecentFiles.IndexOf(path);
			if (index >= 0)
				this.RecentFiles.Move(index, 0);
			else this.RecentFiles.Insert(0, path);
			if (this.RecentFiles.Count > this.MaxRecentFileCount)
				this.RecentFiles.RemoveAt(this.RecentFiles.Count - 1);
			this.Save();
		}

		public void Save()
		{
			string directory = System.IO.Path.GetDirectoryName(SavePath);
			if (!System.IO.Directory.Exists(directory))
				System.IO.Directory.CreateDirectory(directory);

			System.IO.StreamWriter writer = new System.IO.StreamWriter(SavePath);
			//NetJSON.NetJSON.Serialize(this, writer);
			new Newtonsoft.Json.JsonSerializer().Serialize(writer, this);
			writer.Close();
		}
	}
}