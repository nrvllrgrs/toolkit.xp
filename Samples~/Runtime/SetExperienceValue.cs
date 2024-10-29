using Unity.VisualScripting;

namespace ToolkitEngine.XP.VisualScripting
{
	[UnitCategory("Experience")]
	[UnitTitle("Set Value")]
	public class SetExperienceValue : Unit
	{
		#region Ports

		[DoNotSerialize, PortLabelHidden]
		public ControlInput enter;

		[DoNotSerialize, PortLabelHidden]
		public ControlOutput exit;

		[DoNotSerialize, PortLabelHidden]
		public ValueInput xp;

		[DoNotSerialize, PortLabelHidden]
		public ValueInput value;

		#endregion

		#region Methods

		protected override void Definition()
		{
			enter = ControlInput(nameof(enter), Trigger);
			exit = ControlOutput(nameof(exit));
			Succession(enter, exit);

			xp = ValueInput<ExperienceType>(nameof(xp), null);
			Requirement(xp, enter);

			value = ValueInput(nameof(value), 0);
		}

		private ControlOutput Trigger(Flow flow)
		{
			ExperienceManager.CastInstance.SetValue(
				flow.GetValue<ExperienceType>(xp),
				flow.GetValue<int>(value));

			return exit;
		}

		#endregion
	}
}