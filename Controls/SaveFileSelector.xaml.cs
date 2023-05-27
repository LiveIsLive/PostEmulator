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
	/// FileSelector.xaml 的交互逻辑
	/// </summary>
	public partial class SaveFileSelector : Control
	{
		private Microsoft.WindowsAPICodePack.Dialogs.CommonSaveFileDialog _SaveFileDialog;
		protected Microsoft.WindowsAPICodePack.Dialogs.CommonSaveFileDialog SaveFileDialog
		{
			get
			{
				if (_SaveFileDialog == null)
				{
					_SaveFileDialog = new Microsoft.WindowsAPICodePack.Dialogs.CommonSaveFileDialog();
					_SaveFileDialog.DefaultExtension = "zip";
					_SaveFileDialog.AlwaysAppendDefaultExtension = true;
					_SaveFileDialog.Filters.Add(new Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogFilter("*.zip", "*.zip"));
				}
				return _SaveFileDialog;
			}
		}

		[Bindables.DependencyProperty]
		public string OpenButtonText { get; set; } = "选择...";

		[Bindables.DependencyProperty(Options = FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)]
		public string Path { get; set; }

		public SaveFileSelector()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if(!string.IsNullOrWhiteSpace(this.Path))
				this.SaveFileDialog.DefaultFileName = System.IO.Path.GetFileName(this.Path);
			if (this.SaveFileDialog.ShowDialog() == Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogResult.Ok)
				this.Path = this.SaveFileDialog.FileName;
		}
	}
}
