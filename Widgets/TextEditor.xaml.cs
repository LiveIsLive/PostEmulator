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

namespace ColdShineSoft.PostEmulator.Widgets
{
	/// <summary>
	/// TextEditor.xaml 的交互逻辑
	/// </summary>
	public partial class TextEditor : UserControl
	{
		[Bindables.DependencyProperty]
		public string Text { get; set; }

		[Bindables.DependencyProperty]
		public object Type { get; set; }

		public TextEditor()
		{
			InitializeComponent();
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			if(e.Property.Name==nameof(this.Text))
				this.Editor.Text = this.Text;
			else if (e.Property.Name == nameof(this.Type) && this.Type != null)
				this.Editor.SyntaxHighlighting= ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance.GetDefinition(this.Type.ToString());

			base.OnPropertyChanged(e);
		}

		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);
		}

		private void Editor_LostFocus(object sender, RoutedEventArgs e)
		{
			this.Text = this.Editor.Text;
		}
	}
}
