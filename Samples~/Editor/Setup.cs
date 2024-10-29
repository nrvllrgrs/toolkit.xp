using System;
using System.Collections.Generic;
using UnityEditor;
using ToolkitEngine.XP;

namespace ToolkitEditor.XP.VisualScripting
{
	[InitializeOnLoad]
	public static class Setup
	{
		static Setup()
		{
			var types = new List<Type>()
			{
				typeof(ExperienceType),
				typeof(ExperienceEventArgs),
			};

			ToolkitEditor.VisualScripting.Setup.Initialize("ToolkitEngine.XP", types);
		}
	}
}