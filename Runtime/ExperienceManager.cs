using System;
using System.Collections.Generic;
using ToolkitEngine.Inventory;

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

		public static event Action<ExperienceEventArgs> ValueChanged;
		public static event Action<ExperienceEventArgs> LevelChanged;

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

		public static bool TryGetRuntimeExperince(ExperienceType xp, out RuntimeExperienceType runtimeXp)
		{
			return CastInstance.m_runtimeMap.TryGetValue(xp, out runtimeXp);
		}

		public static int GetValue(ExperienceType xp)
		{
			if (!Exists)
				return -1;

			return CastInstance.m_runtimeMap.TryGetValue(xp, out var runtimeXp)
				? runtimeXp.value
				: -1;
		}

		public static void SetValue(ExperienceType xp, int value)
		{
			if (!CastInstance.m_runtimeMap.TryGetValue(xp, out var runtimeXp))
				return;

			runtimeXp.value = value;
		}

		public static void ModifyValue(ExperienceType xp, int delta)
		{
			if (!CastInstance.m_runtimeMap.TryGetValue(xp, out var runtimeXp))
				return;

			runtimeXp.value += delta;
		}

		public static int GetLevel(ExperienceType xp)
		{
			return CastInstance.m_runtimeMap.TryGetValue(xp, out var runtimeXp)
				? runtimeXp.level
				: -1;
		}

		#endregion

		#region Callbacks

		private void RuntimeXp_ValueChanged(object sender, ExperienceEventArgs e)
		{
			ValueChanged?.Invoke(e);
		}

		private void RuntimeXp_LevelChanged(object sender, ExperienceEventArgs e)
		{
			if (m_runtimeMap.TryGetValue(e.experienceType, out var runtimeXp))
			{
				foreach (var drop in e.experienceType.levels[runtimeXp.level - 1].rewards)
				{
					switch (drop.dropType)
					{
						case DropEntry.DropType.Currency:
							// Keep in mind this will only work if already managed
							InventoryManager.ModifyAmount(drop.currencyType, drop.amount);
							break;
					}
				}
			}
			LevelChanged?.Invoke(e);
		}

		#endregion
	}
}