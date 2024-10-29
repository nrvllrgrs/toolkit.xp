namespace ToolkitEngine.XP.VisualScripting
{
	public class OnExperienceValueChanged : BaseExperienceEventUnit
	{
		protected override void StartListeningToManager()
		{
			ExperienceManager.CastInstance.ValueChanged += InvokeTrigger;
		}

		protected override void StopListeningToManager()
		{
			ExperienceManager.CastInstance.ValueChanged -= InvokeTrigger;
		}
	}
}