using UnityEngine;

namespace ToolkitEngine.XP
{
    public class Experience : MonoBehaviour
    {
		#region Fields

		[SerializeField]
		private ExperienceType m_xpType;

		[SerializeField, Min(0)]
		private int m_amount;

		#endregion

		#region Properties

		public int amount { get => m_amount; set => m_amount = value; }

		#endregion

		#region Methods

		[ContextMenu("Award")]
		public void Award()
		{
			if (!ExperienceManager.TryGetRuntimeExperince(m_xpType, out var runtimeXp))
				return;

			runtimeXp.value += m_amount;
		}

		#endregion
	}
}