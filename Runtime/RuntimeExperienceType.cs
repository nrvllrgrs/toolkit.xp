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

		/// <summary>
		/// Gets or sets the current level. Setting this value updates the min/max experience range and raises the LevelChanged event.
		/// </summary>
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

		/// <summary>
		/// Gets the maximum level defined in the ExperienceType configuration.
		/// </summary>
		public int maxLevel => experience.maxLevel;

		/// <summary>
		/// Gets or sets the total experience value. Setting this value automatically levels up if the next level threshold is reached and raises the ValueChanged event.
		/// </summary>
		public int value
		{
			get => m_value;
			set
			{
				value = Mathf.Min(value, experience.maxValue);
				if (m_value == value)
					return;

				m_value = value;
				if (value >= nextLevelValue)
				{
					++level;
				}

				// Invoke value changed after updating level (which updates min-max range)
				ValueChanged?.Invoke(this, new ExperienceEventArgs(experience));
			}
		}

		/// <summary>
		/// Gets the total experience value required to reach the next level. Returns -1 if already at max level.
		/// </summary>
		public int nextLevelValue
		{
			get
			{
				if (m_level == maxLevel)
					return -1;

				return m_maxValue;
			}
		}

		/// <summary>
		/// Gets the normalized progress toward the next level as a value between 0 and 1. Returns 1 if at max level.
		/// </summary>
		public float normalizedLevelValue
		{
			get
			{
				if (m_level == maxLevel)
					return 1f;

				return MathUtil.GetPercent(m_value, m_minValue, m_maxValue);
			}
		}

		/// <summary>
		/// Gets the experience gained in the current level.
		/// </summary>
		public int levelValue => m_value - m_minValue;

		/// <summary>
		/// Gets the remaining experience needed to reach the next level.
		/// </summary>
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

		/// <summary>
		/// Updates the minimum and maximum experience values based on the current level.
		/// </summary>
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