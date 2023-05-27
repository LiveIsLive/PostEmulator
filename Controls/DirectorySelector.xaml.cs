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
	/// DirectorySelector.xaml 的交互逻辑
	/// </summary>
	public partial class DirectorySelector : Control
	{
		private Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog _FolderDialog;
		protected Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog FolderDialog
		{
			get
			{
				if(_FolderDialog==null)
				{
					_FolderDialog = new Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog();
					_FolderDialog.IsFolderPicker = true;
				}
				return _FolderDialog;
			}
		}

		public bool IsFolderPicker
		{
			get
			{
				return this.FolderDialog.IsFolderPicker;
			}
			set
			{
				this.FolderDialog.IsFolderPicker = value;
			}
		}

		[Bindables.DependencyProperty]
		public string OpenButtonText { get; set; } = "选择...";

		[Bindables.DependencyProperty(Options =FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)]
		public string Path { get; set; }

		public static readonly RoutedEvent PathChangedEvent = EventManager.RegisterRoutedEvent("PathChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<string>), typeof(DirectorySelector));

		public event RoutedPropertyChangedEventHandler<string> PathChanged
		{
			add
			{
				this.AddHandler(PathChangedEvent, value);
			}
			remove
			{
				this.RemoveHandler(PathChangedEvent, value);
			}
		}

		protected virtual void OnPathChanged(string oldPath,string newPath)
		{
			RoutedPropertyChangedEventArgs<string> args = new RoutedPropertyChangedEventArgs<string>(oldPath, newPath);
			//this.Path = path;
			args.RoutedEvent = PathChangedEvent;
			this.RaiseEvent(args);
			//System.Threading.Tasks.Task.Run(() =>
			//{
			//	System.Threading.Thread.Sleep(100);
			//	System.Windows.Application.Current.Dispatcher.Invoke(() => this.RaiseEvent(args));
			//});
		}

		public DirectorySelector()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (this.FolderDialog.ShowDialog() == Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogResult.Ok)
				this.Path = this.FolderDialog.FileName;
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if(e.Property.Name==nameof(this.Path))
				this.OnPathChanged((string)e.OldValue, (string)e.NewValue);
		}
	}
}
