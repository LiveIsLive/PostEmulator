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
	/// ImageViewer.xaml 的交互逻辑
	/// </summary>
	public partial class ImageViewer : DockPanel
	{
		[Bindables.DependencyProperty]
		public ImageSource Source { get; set; }

		public ImageViewer()
		{
			InitializeComponent();
		}

		private void Full_Click(object sender, RoutedEventArgs e)
		{
			this.Image.Stretch = Stretch.None;
		}

		private void Horizontal_Click(object sender, RoutedEventArgs e)
		{
			this.Image.Stretch = Stretch.Uniform;
			this.Image.Width = this.ActualWidth - SystemParameters.VerticalScrollBarWidth;
			this.Image.Height = double.NaN;
		}

		private void Vertical_Click(object sender, RoutedEventArgs e)
		{
			this.Image.Stretch = Stretch.Uniform;
			this.Image.Width = double.NaN;
			this.Image.Height = this.ActualHeight - SystemParameters.HorizontalScrollBarHeight;
		}
	}
}
