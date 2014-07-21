namespace RED.ViewModels
{
	using Caliburn.Micro;
	using System;

	public class BaseViewModel : Screen
	{
		protected T ParseEnum<T>(string name)
		{
			return (T)Enum.Parse(typeof(T), name, true);
		}

		protected BaseViewModel()
		{

		}
	}
}
