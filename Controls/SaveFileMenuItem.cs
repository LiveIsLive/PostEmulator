namespace ColdShineSoft.HttpClientPerformer.Controls
{
	public partial class SaveFileMenuItem : System.Windows.Controls.MenuItem
	{
		private static Microsoft.WindowsAPICodePack.Dialogs.CommonSaveFileDialog _SaveFileDialog;
		protected Microsoft.WindowsAPICodePack.Dialogs.CommonSaveFileDialog SaveFileDialog
		{
			get
			{
				if (_SaveFileDialog == null)
				{
					_SaveFileDialog = new Microsoft.WindowsAPICodePack.Dialogs.CommonSaveFileDialog();
					_SaveFileDialog.DefaultExtension = "json";
					_SaveFileDialog.AlwaysAppendDefaultExtension = true;
					_SaveFileDialog.Filters.Add(new Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogFilter("*.json", "*.json"));
				}
				return _SaveFileDialog;
			}
		}

		[Bindables.DependencyProperty]
		public string SelectedFilePath { get; set; }

		public static readonly System.Windows.RoutedEvent SaveFileEvent = System.Windows.EventManager.RegisterRoutedEvent("SaveFile", System.Windows.RoutingStrategy.Bubble, typeof(System.Windows.RoutedPropertyChangedEventHandler<string>), typeof(SaveFileMenuItem));

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

		public SaveFileMenuItem()
		{
			this.Style = new System.Windows.Style(typeof(SaveFileMenuItem), (System.Windows.Style)System.Windows.Application.Current.TryFindResource(typeof(System.Windows.Controls.MenuItem)));
		}

		protected virtual void OnSaveFile(string path)
		{
			System.Windows.RoutedPropertyChangedEventArgs<string> args = new System.Windows.RoutedPropertyChangedEventArgs<string>(this.SelectedFilePath, path);
			this.SelectedFilePath = path;
			args.RoutedEvent = SaveFileEvent;
			RaiseEvent(args);
		}

		protected override void OnClick()
		{
			if (this.SaveFileDialog.ShowDialog() == Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogResult.Ok)
				this.OnSaveFile(this.SaveFileDialog.FileName);

			base.OnClick();
		}
	}
}