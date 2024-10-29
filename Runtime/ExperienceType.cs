using System;
using System.Collections.Generic;
using UnityEngine;

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
		private List<int> m_levels = new();

		#endregion

		#region Properties

		public string id => m_id;
		public new string name => m_name;
		public string description => m_description;
		public int maxLevel => m_levels.Count + 1;
		public int maxValue => m_levels[^1];
		public IList<int> levels => m_levels;

		#endregion

		#region Methods

		public bool TryGetRequiredValue(int level, out int value)
		{
			if (0 < level && level <= m_levels.Count)
			{
				value = m_levels[level - 1];
				return true;
			}

			value = 0;
			return false;
		}

		#endregion
	}
}