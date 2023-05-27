using System.Linq;

using Caliburn.Micro;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;

namespace ColdShineSoft.HttpClientPerformer.ViewModels
{
	public class Bootstrapper: Caliburn.Micro.BootstrapperBase
	{
		public Bootstrapper()
		{
			this.Initialize();
		}

		protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
		{
			System.Threading.Thread.CurrentThread.CurrentCulture = Models.Setting.Instance.SelectedCulture;
			System.Threading.Thread.CurrentThread.CurrentUICulture = Models.Setting.Instance.SelectedCulture;


			if (e.Args.Length == 1)
				Main.SetOpeningFilePath(e.Args[0]);

			var config = new Caliburn.Micro.TypeMappingConfiguration
			{
				UseNameSuffixesInMappings = false,
				DefaultSubNamespaceForViewModels = typeof(Main).Namespace,
				DefaultSubNamespaceForViews = "ColdShineSoft.HttpClientPerformer.Views"
			};

			Caliburn.Micro.ViewLocator.ConfigureTypeMappings(config);
			Caliburn.Micro.ViewModelLocator.ConfigureTypeMappings(config);
			base.OnStartup(sender, e);

			this.DisplayRootViewForAsync<Main>();
		}

		protected override System.Collections.Generic.IEnumerable<System.Reflection.Assembly> SelectAssemblies()
		{
			return new[] { System.Reflection.Assembly.GetExecutingAssembly(),System.Reflection.Assembly.GetEntryAssembly() };
		}
	}
}