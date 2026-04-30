using System;
using System.Collections.Generic;
using UnityEngine;
using ToolkitEngine.Inventory;

namespace ToolkitEngine.XP
{
	[CreateAssetMenu(menuName = "Toolkit/XP/Experience")]
	public class ExperienceType : ScriptableObject
    {
		#region Fields

		[SerializeField]
		private string m_id = Guid.NewGuid().ToString();

		[SerializeField]
		private string m_name;

		[SerializeField, Multiline(3)]
		private string m_description;

		[SerializeField]
		private List<Level> m_levels = new();

		#endregion

		#region Properties

		public string id => m_id;
		public new string name => m_name;
		public string description => m_description;
		public int maxLevel => m_levels.Count + 1;
		public int maxValue => m_levels[^1].maxValue;
		public IList<Level> levels => m_levels;

		#endregion

		#region Methods

		public bool TryGetRequiredValue(int level, out int value)
		{
			if (0 < level && level <= m_levels.Count)
			{
				value = m_levels[level - 1].maxValue;
				return true;
			}

			value = 0;
			return false;
		}

		#endregion
	}

	[System.Serializable]
	public class Level
	{
		#region Fields

		[SerializeField]
		private int m_maxValue;

		[SerializeField]
		private List<DropEntry> m_rewards = new();

		#endregion

		#region Properties

		public int maxValue => m_maxValue;
		public DropEntry[] rewards => m_rewards.ToArray();

		#endregion
	}
}