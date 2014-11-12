namespace RED.ViewModels
{
	using Caliburn.Micro;
	using ControlCenter;

	public sealed class ShellViewModel : Conductor<IScreen>
	{
		public ShellViewModel()
		{
			DisplayName = "Rover Engagement Display";
			ActivateItem(new ControlCenterViewModel());
		}
	}
}