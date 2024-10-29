using Unity.VisualScripting;

namespace ToolkitEngine.XP.VisualScripting
{
	[UnitCategory("Experience")]
	[UnitTitle("Modify Value")]
	public class ModifyExperienceValue : Unit
    {
		#region Ports

		[DoNotSerialize, PortLabelHidden]
		public ControlInput enter;

		[DoNotSerialize, PortLabelHidden]
		public ControlOutput exit;

		[DoNotSerialize, PortLabelHidden]
		public ValueInput xp;

		[DoNotSerialize, PortLabelHidden]
		public ValueInput delta;

		#endregion

		#region Methods

		protected override void Definition()
		{
			enter = ControlInput(nameof(enter), Trigger);
			exit = ControlOutput(nameof(exit));
			Succession(enter, exit);

			xp = ValueInput<ExperienceType>(nameof(xp), null);
			Requirement(xp, enter);

			delta = ValueInput(nameof(delta), 0);
		}

		private ControlOutput Trigger(Flow flow)
		{
			ExperienceManager.CastInstance.ModifyValue(
				flow.GetValue<ExperienceType>(xp),
				flow.GetValue<int>(delta));

			return exit;
		}

		#endregion
	}
}