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
using System.Windows.Shapes;



namespace ColdShineSoft.PostEmulator.Views
{
	/// <summary>
	/// Main.xaml 的交互逻辑
	/// </summary>
	public partial class Main : MahApps.Metro.Controls.MetroWindow
	{
		public Main()
		{
			InitializeComponent();
		}

		private void DialogTest_Click(object sender, RoutedEventArgs e)
		{
			MahApps.Metro.Controls.Dialogs.DialogManager.ShowMessageAsync(this,"系统提示","This is a Test！",MahApps.Metro.Controls.Dialogs.MessageDialogStyle.AffirmativeAndNegative,new MahApps.Metro.Controls.Dialogs.MetroDialogSettings { AffirmativeButtonText = "确定", NegativeButtonText = "取消" });
		}

		private void DateTimePicker_Loaded(object sender, RoutedEventArgs e)
		{

		}
	}
}
