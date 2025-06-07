namespace MenuGraph.Editor
{
	using UnityEditor;
	using UnityEngine;
	using UnityEngine.UIElements;
	using VisualElementHelper;

	internal sealed class MenuGraphWindow : EditorWindow
	{
		#region Methods
		#region Statics
		[MenuItem("Window/Menu Graph Editor", priority = -10000)]
		internal static void ShowWindow()
		{
			EditorWindow.CreateWindow<MenuGraphWindow>();
		}
		#endregion Statics

		private void CreateGUI()
		{
			rootVisualElement.LoadUXML();

			MenuGraphCanvas menuGraphCanvas = rootVisualElement.Q<MenuGraphCanvas>();

			MenuGraph currentMenuGraph = SelectMenuGraph();

			menuGraphCanvas.PopulateMenuGraph(currentMenuGraph);
		}

		private MenuGraph SelectMenuGraph()
		{
			// TODO : Currently only take the first MenuGraph found in project.
			string[] assetsGuids = AssetDatabase.FindAssets("t:MenuGraph");

			if (assetsGuids.Length == 0)
			{
				return null;
			}

			string manuGraphPath = AssetDatabase.GUIDToAssetPath(assetsGuids[0]);
			return AssetDatabase.LoadAssetAtPath<MenuGraph>(manuGraphPath);
		}
		#endregion Methods
	}
}