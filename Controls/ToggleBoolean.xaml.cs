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

namespace ColdShineSoft.PostEmulator.Controls
{
	/// <summary>
	/// ToggleBoolean.xaml 的交互逻辑
	/// </summary>
	public partial class ToggleBoolean : Control
	{
		[Bindables.DependencyProperty]
		public object TrueContent { get; set; } = " ";

		[Bindables.DependencyProperty]
		public object FalseContent { get; set; } = " ";

		[Bindables.DependencyProperty(Options = FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)]
		public bool Value { get; set; }

		public ToggleBoolean()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			this.Value = !this.Value;
		}
	}
}
