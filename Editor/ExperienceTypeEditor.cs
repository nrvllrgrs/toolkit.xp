using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using ToolkitEngine.XP;

namespace ToolkitEditor.XP
{
	[CustomEditor(typeof(ExperienceType))]
    public class ExperienceTypeEditor : BaseToolkitEditor
    {
		#region Fields

		protected ExperienceType m_experienceType;

		protected SerializedProperty m_id;
		protected SerializedProperty m_name;
		protected SerializedProperty m_description;
		protected SerializedProperty m_levels;
		private ReorderableList m_levelsList;

		private float m_maxValue = int.MinValue;
		private float m_viewWidth = 1f;

		#endregion

		#region Methods

		private void OnEnable()
		{
			m_experienceType = target as ExperienceType;

			m_id = serializedObject.FindProperty(nameof(m_id));
			m_name = serializedObject.FindProperty(nameof(m_name));
			m_description = serializedObject.FindProperty(nameof(m_description));
			m_levels = serializedObject.FindProperty(nameof(m_levels));

			UpdateMaxValue();
		}

		protected override void DrawProperties()
		{
			if (m_levelsList == null)
			{
				m_levelsList = new ReorderableList(m_experienceType.levels.ToArray(), typeof(int), true, true, true, true);
				m_levelsList.drawHeaderCallback += (Rect rect) =>
				{
					EditorGUI.LabelField(rect, "Levels");
				};
				m_levelsList.drawElementCallback += OnDrawElementCallback;
				m_levelsList.onCanAddCallback += OnCanAddCallback;
				m_levelsList.onAddDropdownCallback += OnAddDropdownCallback;
				m_levelsList.onCanRemoveCallback += OnCanRemoveCallback;
				m_levelsList.onRemoveCallback += OnRemoveCallback;
			}

			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.PropertyField(m_id, new GUIContent("ID"));
			EditorGUI.EndDisabledGroup();

			EditorGUILayout.PropertyField(m_name);
			EditorGUILayout.PropertyField(m_description);

			EditorGUI.BeginChangeCheck();
			m_levelsList.DoLayoutList();

			if (EditorGUI.EndChangeCheck())
			{
				UpdateMaxValue();
			}

			EditorGUILayout.LabelField("Progress");

			// Create space between layout properties and bar graph
			Rect size = GUILayoutUtility.GetLastRect();
			size.y += size.height + EditorGUIUtility.standardVerticalSpacing;
			size.height = size.width;

			if (size.height > 1)
			{
				m_viewWidth = size.height;
			}

			EditorGUILayout.BeginVertical(GUILayout.Height(m_viewWidth));
			{
				GUILayout.FlexibleSpace();

				// Add 2-pixel space between each bar
				float barHeight = (size.width - ((m_levels.arraySize - 1) * 2)) / m_levels.arraySize;
				float barWidth = size.width;

				float heightOffset = size.y + size.width;
				for (int i = 0; i < m_levels.arraySize; ++i)
				{
					float value = m_levels.GetArrayElementAtIndex(i).intValue / m_maxValue;

					// Draw vertical bar
					var pivot = new Vector2(size.x + (barHeight + 2) * i, heightOffset);

					GUIUtility.RotateAroundPivot(-90f, pivot);
					EditorGUI.DrawRect(new Rect(pivot.x, pivot.y, barWidth * value, barHeight), new Color(0.125f, 0.275f, 0.486f));
					GUIUtility.RotateAroundPivot(90f, pivot);
				}
			}
			EditorGUILayout.EndVertical();
		}

		private void UpdateMaxValue()
		{
			m_maxValue = int.MinValue;
			for (int i = 0; i < m_levels.arraySize; ++i)
			{
				m_maxValue = Mathf.Max(m_levels.GetArrayElementAtIndex(i).intValue, m_maxValue);
			}
		}

		#endregion

		#region ReorderableList Methods

		private void OnDrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
		{
			var levelProp = m_levels.GetArrayElementAtIndex(index);
			int levelValue = levelProp.intValue;
			int prevLevelValue = index > 0
				? m_levels.GetArrayElementAtIndex(index - 1).intValue
				: 0;

			Rect position = new Rect(rect);
			position.width = EditorGUIUtility.labelWidth;

			// Draw row label
			EditorGUI.LabelField(position, $"Level {index + 1}");

			position.x += position.width;
			position.width = (rect.width - position.width - EditorGUIUtility.standardVerticalSpacing) / 2f;

			// Draw total value
			levelProp.intValue = EditorGUI.IntField(position, levelValue);

			EditorGUI.BeginDisabledGroup(true);
			{
				position.x += position.width + EditorGUIUtility.standardVerticalSpacing;

				// Draw delta value
				EditorGUI.IntField(position, levelValue - prevLevelValue);
			}
			EditorGUI.EndDisabledGroup();
		}

		private bool OnCanAddCallback(ReorderableList list) => true;

		private void OnAddDropdownCallback(Rect buttonRect, ReorderableList list)
		{
			m_experienceType.levels.Add(0);
			m_levelsList.list = m_experienceType.levels.ToArray();
			UpdateMaxValue();
		}

		private bool OnCanRemoveCallback(ReorderableList list)
		{
			return m_experienceType.levels != null && m_experienceType.levels.Count > 0;
		}

		private void OnRemoveCallback(ReorderableList list)
		{
			// Remove faction from list
			m_experienceType.levels.RemoveAt(list.index);
			list.list = m_experienceType.levels.ToArray();
			UpdateMaxValue();
		}

		#endregion
	}
}