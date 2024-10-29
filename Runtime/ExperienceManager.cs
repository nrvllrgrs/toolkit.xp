using System;
using System.Collections.Generic;

namespace ToolkitEngine.XP
{
	public class ExperienceEventArgs : EventArgs
	{
		#region Properties

		public ExperienceType experienceType { get; private set; }

		#endregion

		#region Constructors

		public ExperienceEventArgs(ExperienceType xp)
		{
			experienceType = xp;
		}

		#endregion
	}

	public class ExperienceManager : ConfigurableSubsystem<ExperienceManager, ExperienceManagerConfig>
    {
		#region Fields

		private Dictionary<ExperienceType, RuntimeExperienceType> m_runtimeMap;

		#endregion

		#region Events

		public event EventHandler<ExperienceEventArgs> ValueChanged;
		public event EventHandler<ExperienceEventArgs> LevelChanged;

		#endregion

		#region Methods

		protected override void Initialize()
		{
			m_runtimeMap = new();

			foreach (var xp in Config.experienceTypes)
			{
				var runtimeXp = new RuntimeExperienceType(xp);
				runtimeXp.ValueChanged += RuntimeXp_ValueChanged;
				runtimeXp.LevelChanged += RuntimeXp_LevelChanged;

				m_runtimeMap.Add(xp, runtimeXp);
			}
		}

		protected override void Terminate()
		{
			foreach (var runtimeCategory in m_runtimeMap.Values)
			{
				runtimeCategory.Dispose();
			}
			m_runtimeMap = null;
		}

		public bool TryGetRuntimeExperince(ExperienceType xp, out RuntimeExperienceType runtimeXp)
		{
			return m_runtimeMap.TryGetValue(xp, out runtimeXp);
		}

		public int GetValue(ExperienceType xp)
		{
			return m_runtimeMap.TryGetValue(xp, out var runtimeXp)
				? runtimeXp.value
				: -1;
		}

		public void SetValue(ExperienceType xp, int value)
		{
			if (!m_runtimeMap.TryGetValue(xp, out var runtimeXp))
				return;

			runtimeXp.value = value;
		}

		public void ModifyValue(ExperienceType xp, int delta)
		{
			if (!m_runtimeMap.TryGetValue(xp, out var runtimeXp))
				return;

			runtimeXp.value += delta;
		}

		public int GetLevel(ExperienceType xp)
		{
			return m_runtimeMap.TryGetValue(xp, out var runtimeXp)
				? runtimeXp.level
				: -1;
		}

		#endregion

		#region Callbacks

		private void RuntimeXp_ValueChanged(object sender, ExperienceEventArgs e)
		{
			ValueChanged?.Invoke(null, e);
		}

		private void RuntimeXp_LevelChanged(object sender, ExperienceEventArgs e)
		{
			LevelChanged?.Invoke(null, e);
		}

		#endregion
	}
}