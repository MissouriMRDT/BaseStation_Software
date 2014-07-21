namespace RED
{
	using System.Threading;
	using Caliburn.Micro;
	using System.Windows;
	using ViewModels;
	using Views;

	public class RedBootstrapper : BootstrapperBase
	{
		public RedBootstrapper()
		{
			Initialize();
		}

		protected override void OnStartup(object sender, StartupEventArgs e)
		{
			//Disable shutdown when the dialog closes
			Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
			var dialog = new StartupStatusView();
			dialog.Show();
			// Do initialization work here...
			dialog.Close();
			Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

			DisplayRootViewFor<ShellViewModel>();
		}
	}
}
