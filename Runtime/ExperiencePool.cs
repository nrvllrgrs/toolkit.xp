using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace ToolkitEngine.XP
{
    public class ExperiencePool : MonoBehaviour
    {
		#region Fields

		[SerializeField, Required]
		private ExperienceType m_xpType;

		private RuntimeExperienceType m_runtimeXp;

		#endregion

		#region Events

		[SerializeField, Foldout("Events")]
		private UnityEvent<int> m_onValueChanged;

		[SerializeField, Foldout("Events")]
		private UnityEvent<int> m_onLevelChanged;

		#endregion

		#region Properties

		public int level => m_runtimeXp.level;
		public int maxLevel => m_runtimeXp.maxLevel;
		public int value { get => m_runtimeXp.value; set => m_runtimeXp.value = value; }
		public int nextLevelValue => m_runtimeXp.nextLevelValue;
		public float normalizedLevelValue => m_runtimeXp.normalizedLevelValue;
		public int levelValue => m_runtimeXp.levelValue;
		public int remainingLevelValue => m_runtimeXp.remainingLevelValue;

		public UnityEvent<int> onValueChanged => m_onValueChanged;
		public UnityEvent<int> onLevelChanged => m_onLevelChanged;

		#endregion

		#region Methods

		private void Awake()
		{
			ExperienceManager.TryGetRuntimeExperince(m_xpType, out m_runtimeXp);
		}

		private void OnEnable()
		{
			if (m_runtimeXp == null)
				return;

			m_runtimeXp.ValueChanged += RuntimeXp_ValueChanged;
			m_runtimeXp.LevelChanged += RuntimeXp_LevelChanged;
		}

		private void OnDisable()
		{
			if (m_runtimeXp == null)
				return;

			m_runtimeXp.ValueChanged -= RuntimeXp_ValueChanged;
			m_runtimeXp.LevelChanged -= RuntimeXp_LevelChanged;
		}

		#endregion

		#region Callbacks

		private void RuntimeXp_ValueChanged(object sender, ExperienceEventArgs e)
		{
			m_onValueChanged?.Invoke(m_runtimeXp.value);
		}

		private void RuntimeXp_LevelChanged(object sender, ExperienceEventArgs e)
		{
			m_onLevelChanged?.Invoke(m_runtimeXp.level);
		}

		#endregion
	}
}