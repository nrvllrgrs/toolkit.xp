namespace ToolkitEngine.XP.VisualScripting
{
	public class OnExperienceLevelChanged : BaseExperienceEventUnit
	{
		protected override void StartListeningToManager()
		{
			ExperienceManager.CastInstance.LevelChanged += InvokeTrigger;
		}

		protected override void StopListeningToManager()
		{
			ExperienceManager.CastInstance.LevelChanged -= InvokeTrigger;
		}
	}
}