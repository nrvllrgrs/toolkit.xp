using System;
using UnityEngine;

namespace ToolkitEngine.XP
{
	[Serializable]
	public class RuntimeExperienceType : IDisposable
	{
		#region Fields

		private int m_value;
		private int m_level = 1;
		private int m_minValue, m_maxValue;

		private bool m_disposed;

		#endregion

		#region Events

		public event EventHandler<ExperienceEventArgs> ValueChanged;
		public event EventHandler<ExperienceEventArgs> LevelChanged;

		#endregion

		#region Properties

		public ExperienceType experience { get; private set; }

		public int level
		{
			get => m_level;
			set
			{
				// No change, skip
				if (m_level == value)
					return;

				m_level = value;

				// Update the min-max range before notifications so normalized value is correct
				UpdateMinMax();

				LevelChanged?.Invoke(this, new ExperienceEventArgs(experience));
			}
		}

		public int maxLevel => experience.maxLevel;

		public int value
		{
			get => m_value;
			set
			{
				value = Mathf.Min(value, experience.maxValue);
				if (m_value == value)
					return;

				m_value = value;
				ValueChanged?.Invoke(this, new ExperienceEventArgs(experience));

				if (value > nextLevelValue)
				{
					++level;
				}
			}
		}

		public int nextLevelValue
		{
			get
			{
				if (m_level == maxLevel)
					return -1;

				return m_maxValue;
			}
		}

		public float normalizedLevelValue
		{
			get
			{
				if (m_level == maxLevel)
					return 1f;

				return MathUtil.GetPercent(m_value, m_minValue, m_maxValue);
			}
		}

		public int levelValue => m_value - m_minValue;
		public int remainingLevelValue => m_maxValue - m_value;

		#endregion

		#region Constructors

		public RuntimeExperienceType(ExperienceType xp)
		{
			experience = xp;
			m_minValue = 0;
			experience.TryGetRequiredValue(m_level, out m_maxValue);
		}

		#endregion

		#region Methods

		private void UpdateMinMax()
		{
			if (m_level == 1 || !experience.TryGetRequiredValue(m_level - 1, out m_minValue))
			{
				m_minValue = 0;
			}

			experience.TryGetRequiredValue(m_level, out m_maxValue);
		}

		#endregion

		#region IDisposable Methods

		~RuntimeExperienceType()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (m_disposed)
				return;

			m_disposed = true;
		}

		#endregion
	}
}