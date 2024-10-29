using ToolkitEngine.VisualScripting;
using Unity.VisualScripting;

namespace ToolkitEngine.XP.VisualScripting
{
	[UnitCategory("Events/Experience")]
	public abstract class BaseExperienceEventUnit : FilteredManagerEventUnit<ExperienceEventArgs, ExperienceType>
	{
		#region Methods

		protected override ExperienceType GetFilterValue(ExperienceEventArgs args) => args.experienceType;

		#endregion
	}
}