using UnityEngine;

namespace ToolkitEngine.XP
{
    [CreateAssetMenu(menuName = "Toolkit/Config/ExperienceManager Config")]
    public class ExperienceManagerConfig : ScriptableObject
    {
		#region Fields

		[SerializeField]
		private ExperienceType[] m_experienceTypes;

		#endregion

		#region Propertes

		public ExperienceType[] experienceTypes => m_experienceTypes;

		#endregion
	}
}