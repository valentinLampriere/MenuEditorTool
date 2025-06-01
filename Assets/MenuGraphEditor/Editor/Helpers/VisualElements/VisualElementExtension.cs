namespace VisualElementHelper
{
	using System.IO;
	using System.Runtime.CompilerServices;
	using UnityEditor;
	using UnityEngine;
	using UnityEngine.UIElements;

	public static class VisualElementExtension
	{
		#region Constants
		public const string VISUAL_TREE_EXT = "uxml";
		public const string STYLE_SHEET_EXT = "uss";
		#endregion Constants

		#region Methods
		public static void LoadUXML(this VisualElement visualElement, [CallerFilePath] string absoluteScriptFilePath = "")
		{
			string currentFilePath = $"Assets" + absoluteScriptFilePath.Substring(Application.dataPath.Length);
			string uxmlPath = Path.ChangeExtension(currentFilePath, VISUAL_TREE_EXT);
			VisualTreeAsset uxml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(uxmlPath);

			if (uxml == null)
			{
				throw new FileNotFoundException($"Couldn't find a UXML asset at this location \"{uxmlPath}\"");
			}

			uxml.CloneTree(visualElement);
		}

		public static void LoadUSS(this VisualElement visualElement, [CallerFilePath] string absoluteScriptFilePath = "")
		{
			string currentFilePath = $"Assets" + absoluteScriptFilePath.Substring(Application.dataPath.Length);
			string ussPath = Path.ChangeExtension(currentFilePath, STYLE_SHEET_EXT);
			StyleSheet uss = AssetDatabase.LoadAssetAtPath<StyleSheet>(ussPath);

			if (uss == null)
			{
				throw new FileNotFoundException($"Couldn't find a USS asset at this location \"{ussPath}\"");
			}

			visualElement.styleSheets.Add(uss);
		}
		#endregion Methods
	}
}