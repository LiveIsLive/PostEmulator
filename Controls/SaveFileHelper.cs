//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ColdShineSoft.HttpClientPerformer.Controls
//{
//	public class SaveFileHelper: System.Windows.DependencyObject
//	{
//		private static Microsoft.WindowsAPICodePack.Dialogs.CommonSaveFileDialog _SaveFileDialog;
//		private static Microsoft.WindowsAPICodePack.Dialogs.CommonSaveFileDialog SaveFileDialog
//		{
//			get
//			{
//				if (_SaveFileDialog == null)
//				{
//					_SaveFileDialog = new Microsoft.WindowsAPICodePack.Dialogs.CommonSaveFileDialog();
//					_SaveFileDialog.DefaultExtension = "json";
//					_SaveFileDialog.AlwaysAppendDefaultExtension = true;
//					_SaveFileDialog.Filters.Add(new Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogFilter("*.json", "*.json"));
//				}
//				return _SaveFileDialog;
//			}
//		}


//		public static readonly System.Windows.RoutedEvent SaveFileEvent = System.Windows.EventManager.RegisterRoutedEvent("SaveFile", System.Windows.RoutingStrategy.Bubble, typeof(System.Windows.RoutedPropertyChangedEventHandler<string>), typeof(SaveFileHelper));

//		private static void OnSaveFile(System.Windows.UIElement element, string path)
//		{
//			System.Windows.RoutedPropertyChangedEventArgs<string> args = new System.Windows.RoutedPropertyChangedEventArgs<string>(GetSelectedFilePath(element), path);
//			SetSelectedFilePath(element, path);
//			args.RoutedEvent = SaveFileEvent;
//			element.RaiseEvent(args);
//		}

//		private static void OnSaveFileClick(System.Windows.UIElement element)
//		{
//			if (SaveFileDialog.ShowDialog() == Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogResult.Ok)
//				OnSaveFile(element, SaveFileDialog.FileName);
//		}

//		public static void AddSaveFileHandler(System.Windows.DependencyObject d, System.Windows.RoutedPropertyChangedEventHandler<string> handler)
//		{
//			((System.Windows.UIElement)d).AddHandler(SaveFileEvent, handler);
//		}

//		public static void RemoveSaveFileHandler(System.Windows.DependencyObject d, System.Windows.RoutedPropertyChangedEventHandler<string> handler)
//		{
//			((System.Windows.UIElement)d).RemoveHandler(SaveFileEvent, handler);
//		}

//		[Bindables.AttachedProperty(Options =System.Windows.FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)]
//		public static string SelectedFilePath { get; set; }

//		public static string GetSelectedFilePath(System.Windows.DependencyObject obj)
//		{
//			// This method has to have the only line below.
//			throw new Bindables.WillBeImplementedByBindablesException();
//		}
//		public static void SetSelectedFilePath(System.Windows.DependencyObject obj, string value)
//		{
//			// This method has to be empty.
//		}

//		[Bindables.AttachedProperty(OnPropertyChanged = nameof(EnabledChanged))]
//		public static bool Enabled { get; set; }

//		public static bool GetEnabled(System.Windows.DependencyObject obj)
//		{
//			// This method has to have the only line below.
//			throw new Bindables.WillBeImplementedByBindablesException();
//		}
//		public static void SetEnabled(System.Windows.DependencyObject obj, bool value)
//		{
//			// This method has to be empty.
//		}

//		private static void EnabledChanged(System.Windows.DependencyObject target, System.Windows.DependencyPropertyChangedEventArgs e)
//		{
//			if((bool)e.NewValue)
//			{
//				if (target is System.Windows.Controls.Primitives.ButtonBase)
//				{
//					System.Windows.Controls.Primitives.ButtonBase button = (System.Windows.Controls.Primitives.ButtonBase)target;
//					button.AddHandler(System.Windows.Controls.Primitives.ButtonBase.ClickEvent, ClickHandler);
//					return;
//				}
//				if (target is System.Windows.Controls.MenuItem)
//				{
//					System.Windows.Controls.MenuItem menuItem = (System.Windows.Controls.MenuItem)target;
//					menuItem.AddHandler(System.Windows.Controls.MenuItem.ClickEvent, ClickHandler);
//					return;
//				}
//			}
//			else
//			{
//				if (target is System.Windows.Controls.Primitives.ButtonBase)
//				{
//					System.Windows.Controls.Primitives.ButtonBase button = (System.Windows.Controls.Primitives.ButtonBase)target;
//					button.AddHandler(System.Windows.Controls.Primitives.ButtonBase.ClickEvent, ClickHandler);
//					return;
//				}
//				if (target is System.Windows.Controls.MenuItem)
//				{
//					System.Windows.Controls.MenuItem menuItem = (System.Windows.Controls.MenuItem)target;
//					menuItem.AddHandler(System.Windows.Controls.MenuItem.ClickEvent, ClickHandler);
//					return;
//				}
//			}
//		}

//		private static System.Windows.RoutedEventHandler ClickHandler = (sender,args)=> OnSaveFileClick((System.Windows.UIElement)sender);
//	}
//}