using Unity.VisualScripting;

namespace ToolkitEngine.XP.VisualScripting
{
	[UnitCategory("Experience")]
	[UnitTitle("Get Value")]
	public class GetExperienceValue : Unit
	{
		#region Ports

		[DoNotSerialize, PortLabelHidden]
		public ControlInput enter;

		[DoNotSerialize, PortLabelHidden]
		public ControlOutput exit;

		[DoNotSerialize, PortLabelHidden]
		public ValueInput xp;

		[DoNotSerialize, PortLabelHidden]
		public ValueOutput value;

		#endregion

		#region Methods

		protected override void Definition()
		{
			enter = ControlInput(nameof(enter), Trigger);
			exit = ControlOutput(nameof(exit));
			Succession(enter, exit);

			xp = ValueInput<ExperienceType>(nameof(xp), null);
			Requirement(xp, enter);

			value = ValueOutput(nameof(value), (flow) => ExperienceManager.CastInstance.GetValue(flow.GetValue<ExperienceType>(xp)));
		}

		private ControlOutput Trigger(Flow flow)
		{
			return exit;
		}

		#endregion
	}
}