using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ColdShineSoft.HttpClientPerformer.Controls
{
	public class DropDownButton : MahApps.Metro.Controls.SplitButton
	{
		private System.Windows.Controls.ContentControl _InnerButton;
		protected System.Windows.Controls.ContentControl InnerButton
		{
			get
			{
				if (this._InnerButton == null)
				{
					this._InnerButton = (System.Windows.Controls.ContentControl)this.Template.FindName("PART_ButtonContent", this);
					this._InnerButton.VerticalAlignment = System.Windows.VerticalAlignment.Center;
				}
				return this._InnerButton;
			}
		}

		private MahApps.Metro.IconPacks.PackIconCodicons _IconControl;
		protected MahApps.Metro.IconPacks.PackIconCodicons IconControl
		{
			get
			{
				if(this._IconControl==null)
				{
					this._IconControl = new MahApps.Metro.IconPacks.PackIconCodicons();
					this._IconControl.Margin = new Thickness(5, 0, 3, 0);
					this._IconControl.VerticalAlignment = System.Windows.VerticalAlignment.Center;

					System.Windows.Data.Binding binding = new System.Windows.Data.Binding("FontSize");
					binding.Source = this;
					this._IconControl.SetBinding(MahApps.Metro.IconPacks.PackIconCodicons.WidthProperty, binding);
					this._IconControl.SetBinding(MahApps.Metro.IconPacks.PackIconCodicons.HeightProperty, binding);
				}
				return this._IconControl;
			}
		}

		public MahApps.Metro.IconPacks.PackIconCodiconsKind Kind
		{
			get
			{
				return this.IconControl.Kind;
			}
			set
			{
				this.IconControl.Kind = value;
			}
		}

		public DropDownButton()
		{
			this.FontFamily = new System.Windows.Media.FontFamily("微软雅黑");
			this.Icon = this.IconControl;
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);

			//if (e.Property == TextProperty)
			//	if (this.InnerButton != null)
			//		this.InnerButton.Content = e.NewValue;
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			//this.InnerButton.Content = this.Text;
			System.Windows.Data.Binding binding = new System.Windows.Data.Binding("Text");
			binding.Source = this;
			this.InnerButton.SetBinding(System.Windows.Controls.ContentControl.ContentProperty, binding);
		}

		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);
		}

		protected override void OnSelectionChanged(SelectionChangedEventArgs e)
		{
			e.Handled = true;
			//base.OnSelectionChanged(e);
		}
	}
}