using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ColdShineSoft.PostEmulator.Controls
{
	public class TabControl : MahApps.Metro.Controls.MetroTabControl
	{
		[Bindables.DependencyProperty(OnPropertyChanged =nameof(OnIsFocusChanged),Options =System.Windows.FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)]
		public bool IsFocus { get; set; }

		protected static void OnIsFocusChanged(System.Windows.DependencyObject dependencyObject,System.Windows.DependencyPropertyChangedEventArgs eventArgs)
		{
			if (!(bool)eventArgs.NewValue)
				return;

			TabControl tab=(TabControl)dependencyObject;
			tab.Focus();
			tab.IsFocus = false;
		}

		public TabControl()
		{
			//this.Style = new System.Windows.Style(typeof(HandyControl.Controls.TabControl), (System.Windows.Style)System.Windows.Application.Current.TryFindResource(typeof(HandyControl.Controls.TabControl)));
		}

		protected override void OnSelectionChanged(SelectionChangedEventArgs e)
		{
			this.Focus();
			base.OnSelectionChanged(e);
		}
	}
}