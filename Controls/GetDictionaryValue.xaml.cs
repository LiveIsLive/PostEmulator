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
	/// GetDictionaryValue.xaml 的交互逻辑
	/// </summary>
	public partial class GetDictionaryValue : Control
	{
		[Bindables.DependencyProperty]
		public System.Collections.IDictionary Dictionary { get; set; }

		[Bindables.DependencyProperty]
		public object Key { get; set; }

		[Bindables.DependencyProperty]
		public object Text { get; set; }

		public GetDictionaryValue()
		{
			InitializeComponent();
		}

		protected static readonly string[] Properties = new string[] { nameof(Dictionary), nameof(Key) };

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (Properties.Contains(e.Property.Name))
				this.SetText();
		}

		protected void SetText()
		{
			if (this.Dictionary == null || this.Key == null)
				this.Text = "";
			else if (this.Dictionary.Contains(this.Key))
				this.Text = System.Convert.ToString(this.Dictionary[this.Key]);
			else this.Text = this.Key;
		}
	}
}
