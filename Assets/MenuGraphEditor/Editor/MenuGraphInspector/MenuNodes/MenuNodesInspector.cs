namespace MenuGraph.Editor
{
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;
	using UnityEngine.UIElements;
	using VisualElementHelper;

	[UxmlElement]
	internal sealed partial class MenuNodesInspector : VisualElement
	{
		#region Constants
		private const string MENUS_HIERARCHY_UXML_ID = "MenusHierarchy";

		private const char SEPARATOR = '/';
		#endregion Constants

		#region Fields
		private VisualElement _menusHierarchyRoot = null;
		private MenuNodesHierarchy _menuNodesHierarchy = null;
		#endregion Fields

		#region Constructors
		public MenuNodesInspector()
		{
			this.LoadUXML();

			_menusHierarchyRoot = this.Q(MENUS_HIERARCHY_UXML_ID);
			_menuNodesHierarchy = new MenuNodesHierarchy();
			_menusHierarchyRoot.Add(_menuNodesHierarchy);

			FillMenusHierarchy();
		}
		#endregion Constructors

		#region Methods
		private void FillMenusHierarchy()
		{
			string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab");
			int prefabsCount = prefabGuids.Length;
			for (int i = 0; i < prefabsCount; i++)
			{
				string prefabGuid = prefabGuids[i];
				string path = AssetDatabase.GUIDToAssetPath(prefabGuid);
				GameObject menuNodePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

				if (menuNodePrefab != null && menuNodePrefab.GetComponent<MenuNode>() != null)
				{
					_menuNodesHierarchy.AddMenuPrefab(path);
				}
			}
		}
		#endregion Methods
	}
}