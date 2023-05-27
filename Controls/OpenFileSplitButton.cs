using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ColdShineSoft.HttpClientPerformer.Controls
{
	/// <summary>
	/// OpenFileButton.xaml 的交互逻辑
	/// </summary>
	public partial class OpenFileSplitButton : DropDownButton
	{
		private static Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog _OpenFileDialog;
		protected Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog OpenFileDialog
		{
			get
			{
				if (_OpenFileDialog == null)
				{
					_OpenFileDialog = new Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog();
					_OpenFileDialog.Filters.Add(new Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogFilter("*.json", "*.json"));
				}
				return _OpenFileDialog;
			}
		}

		[Bindables.DependencyProperty]
		public string SelectedFilePath { get; set; }

		public static readonly RoutedEvent OpenFileEvent = EventManager.RegisterRoutedEvent("OpenFile", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<string>), typeof(OpenFileSplitButton));

		public event RoutedPropertyChangedEventHandler<string> OpenFile
		{
			add
			{
				this.AddHandler(OpenFileEvent, value);
			}
			remove
			{
				this.RemoveHandler(OpenFileEvent, value);
			}
		}

		public OpenFileSplitButton()
		{
			//this.Style = new Style(typeof(OpenFileSplitButton), (System.Windows.Style)Application.Current.TryFindResource(typeof(HandyControl.Controls.SplitButton)));
			this.Click += OpenFileSplitButton_Click;
		}

		protected virtual void OnOpenFile(string path)
		{
			RoutedPropertyChangedEventArgs<string> args = new RoutedPropertyChangedEventArgs<string>(this.SelectedFilePath, path);
			this.SelectedFilePath = path;
			args.RoutedEvent = OpenFileEvent;
			RaiseEvent(args);
		}

		private void OpenFileSplitButton_Click(object sender, RoutedEventArgs e)
		{
			if (this.OpenFileDialog.ShowDialog() == Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogResult.Ok)
				this.OnOpenFile(this.OpenFileDialog.FileName);
		}
	}
}
