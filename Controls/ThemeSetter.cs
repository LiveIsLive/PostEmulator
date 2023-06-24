using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.PostEmulator.Controls
{
	public class ThemeSetter:System.Windows.FrameworkElement
	{
		[Bindables.DependencyProperty(OnPropertyChanged = nameof(OnThemeChanged))]
		public string Theme { get; set; }


		protected static void OnThemeChanged(System.Windows.DependencyObject dependencyObject, System.Windows.DependencyPropertyChangedEventArgs eventArgs)
		{
			if (eventArgs.NewValue != null)
				ControlzEx.Theming.ThemeManager.Current.ChangeTheme(System.Windows.Application.Current, eventArgs.NewValue.ToString());
		}

	}
}