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
	/// IconButton.xaml 的交互逻辑
	/// </summary>
	public partial class IconButton : Button
	{
		//[Bindables.DependencyProperty]
		public MahApps.Metro.IconPacks.PackIconCodiconsKind Icon { get; set; }

		//static IconButton()
		//{
		//	DefaultStyleKeyProperty.OverrideMetadata(typeof(IconButton), new FrameworkPropertyMetadata(typeof(IconButton)));
		//}

		public IconButton()
		{
			InitializeComponent();
		}
	}
}
