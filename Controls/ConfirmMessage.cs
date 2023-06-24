using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.PostEmulator.Controls
{
	public class ConfirmMessage:System.Windows.FrameworkElement
	{
		[Bindables.DependencyProperty]
		public string Title { get; set; } = "Confirm";

		[Bindables.DependencyProperty]
		public string Message { get; set; }

		[Bindables.DependencyProperty]
		public string OkText { get; set; }

		[Bindables.DependencyProperty]
		public string CancelText { get; set; }

		[Bindables.DependencyProperty(OnPropertyChanged = nameof(OnShowChanged), Options = System.Windows.FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)]
		public bool Show { get; set; }

		[Bindables.DependencyProperty(Options = System.Windows.FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)]
		public bool Result { get; set; }

		protected static void OnShowChanged(System.Windows.DependencyObject dependencyObject, System.Windows.DependencyPropertyChangedEventArgs eventArgs)
		{
			ConfirmMessage confirmMessage = (ConfirmMessage)dependencyObject;
			if (!confirmMessage.Show)
				return;

			confirmMessage.Result = MahApps.Metro.Controls.Dialogs.DialogManager.ShowModalMessageExternal((MahApps.Metro.Controls.MetroWindow)System.Windows.Window.GetWindow(confirmMessage), confirmMessage.Title, confirmMessage.Message, MahApps.Metro.Controls.Dialogs.MessageDialogStyle.AffirmativeAndNegative, new MahApps.Metro.Controls.Dialogs.MetroDialogSettings { AffirmativeButtonText = confirmMessage.OkText, NegativeButtonText = confirmMessage.CancelText }) == MahApps.Metro.Controls.Dialogs.MessageDialogResult.Affirmative;
			confirmMessage.Show = false;
		}
	}
}