using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.PostEmulator.Controls
{
	public class SaveFileButton : IconButton
	{
		private Microsoft.WindowsAPICodePack.Dialogs.CommonSaveFileDialog _SaveFileDialog;
		protected Microsoft.WindowsAPICodePack.Dialogs.CommonSaveFileDialog SaveFileDialog
		{
			get
			{
				if (_SaveFileDialog == null)
					_SaveFileDialog = new Microsoft.WindowsAPICodePack.Dialogs.CommonSaveFileDialog();
				return _SaveFileDialog;
			}
		}

		[Bindables.DependencyProperty(Options = System.Windows.FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)]
		public string SelectedFilePath { get; set; }

		[Bindables.DependencyProperty]
		public string DefaultFileName { get; set; }

		public static readonly System.Windows.RoutedEvent SaveFileEvent = System.Windows.EventManager.RegisterRoutedEvent("SaveFile", System.Windows.RoutingStrategy.Bubble, typeof(System.Windows.RoutedPropertyChangedEventHandler<string>), typeof(SaveFileButton));

		public event System.Windows.RoutedPropertyChangedEventHandler<string> SaveFile
		{
			add
			{
				this.AddHandler(SaveFileEvent, value);
			}
			remove
			{
				this.RemoveHandler(SaveFileEvent, value);
			}
		}
		public SaveFileButton()
		{
			this.Icon = MahApps.Metro.IconPacks.PackIconCodiconsKind.Save;
			this.Click += SaveFileButton_Click;
		}

		private void SaveFileButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			this.SaveFileDialog.DefaultFileName = this.DefaultFileName;
			if (this.SelectedFilePath == null)
			{
				if (this.SaveFileDialog.ShowDialog() == Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogResult.Ok)
					this.OnSaveFile(this.SaveFileDialog.FileName);
				this._SaveFileDialog = null;
			}
			else this.OnSaveFile(this.SelectedFilePath);
		}

		protected virtual void OnSaveFile(string path)
		{
			System.Windows.RoutedPropertyChangedEventArgs<string> args = new System.Windows.RoutedPropertyChangedEventArgs<string>(this.SelectedFilePath, path);
			this.SelectedFilePath = path;
			args.RoutedEvent = SaveFileEvent;
			RaiseEvent(args);
		}
	}
}