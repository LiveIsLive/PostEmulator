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

namespace ColdShineSoft.HttpClientPerformer.Widgets
{
	/// <summary>
	/// RequestItemEditor.xaml 的交互逻辑
	/// </summary>
	public partial class RequestItemEditor : Control
	{
		[Bindables.DependencyProperty]
		public object Names { get; set; }

		[Bindables.DependencyProperty]
		public System.Collections.ObjectModel.ObservableCollection<Models.RequestItem> Items { get; set; }

		[Bindables.DependencyProperty]
		public Models.RequestItem FirstItem { get; set; }

		[Bindables.DependencyProperty]
		public Models.RequestItem LastItem { get; set; }

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			if (e.Property.Name == nameof(this.Items) && this.Items != null)
			{
				this.FirstItem = this.Items?.FirstOrDefault();
				this.LastItem = this.Items?.LastOrDefault();
				this.Items.CollectionChanged += Items_CollectionChanged;
			}

			base.OnPropertyChanged(e);
		}

		private void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			this.FirstItem = this.Items?.FirstOrDefault();
			this.LastItem = this.Items?.LastOrDefault();
		}

		public RequestItemEditor()
		{
			InitializeComponent();
		}

		private void Add_Click(object sender, RoutedEventArgs e)
		{
			this.Items.Add(new Models.RequestItem());
		}

		private void MoveUp_Click(object sender, RoutedEventArgs e)
		{
			Models.RequestItem item = (Models.RequestItem)((Controls.IconButton)sender).DataContext;
			int index = this.Items.IndexOf(item);
			if (index > 0)
				this.Items.Move(index, index - 1);
		}

		private void MoveDown_Click(object sender, RoutedEventArgs e)
		{
			Models.RequestItem item = (Models.RequestItem)((Controls.IconButton)sender).DataContext;
			int index = this.Items.IndexOf(item);
			if (index < this.Items.Count - 1)
				this.Items.Move(index, index + 1);
		}

		private void Remove_Click(object sender, RoutedEventArgs e)
		{
			this.Items.Remove((Models.RequestItem)((Controls.IconButton)sender).DataContext);
		}
	}
}
