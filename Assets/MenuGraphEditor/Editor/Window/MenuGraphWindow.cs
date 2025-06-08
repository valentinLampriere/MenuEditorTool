namespace MenuGraph.Editor
{
	using UnityEditor;
	using UnityEngine.UIElements;
	using VisualElementHelper;

	internal sealed class MenuGraphWindow : EditorWindow
	{
		#region Fields
		private MenuGraphToolbar _menuGraphToolbar = null;
		private MenuNodesInspector _menuNodesInspector = null;
		private MenuGraphCanvas _menuGraphCanvas = null;
		#endregion Fields

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

			_menuGraphToolbar = rootVisualElement.Q<MenuGraphToolbar>();
			_menuNodesInspector = rootVisualElement.Q<MenuNodesInspector>();
			_menuGraphCanvas = rootVisualElement.Q<MenuGraphCanvas>();

			MenuGraph currentMenuGraph = SelectMenuGraph();

			_menuGraphCanvas.PopulateMenuGraph(currentMenuGraph);
		}

		private void OnDestroy()
		{
			_menuGraphToolbar?.Dispose();
			_menuGraphToolbar = null;

			_menuNodesInspector?.Dispose();
			_menuNodesInspector = null;

			_menuGraphCanvas?.Dispose();
			_menuGraphCanvas = null;
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