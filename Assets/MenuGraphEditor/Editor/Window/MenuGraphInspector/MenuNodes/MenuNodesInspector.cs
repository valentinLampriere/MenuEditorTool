namespace MenuGraph.Editor
{
	using System;
	using UnityEditor;
	using UnityEngine;
	using UnityEngine.UIElements;
	using VisualElementHelper;

	[UxmlElement]
	// TODO : Rename to MenuGraphInspector.
	internal sealed partial class MenuNodesInspector : VisualElement, IDisposable
	{
		#region Constants
		private const string MENUS_HIERARCHY_UXML_ID = "MenusHierarchy";

		private const char SEPARATOR = '/';
		#endregion Constants

		#region Fields
		private MenuNodesHierarchy _menuNodesHierarchy = null;
		#endregion Fields

		#region Constructors
		public MenuNodesInspector()
		{
			this.LoadUXML();

			_menuNodesHierarchy = new MenuNodesHierarchy();
			VisualElement menusHierarchyRoot = this.Q(MENUS_HIERARCHY_UXML_ID);
			menusHierarchyRoot.Add(_menuNodesHierarchy);

			FillMenusHierarchy();
		}
		#endregion Constructors

		#region Methods
		#region IDisposable
		public void Dispose()
		{
			_menuNodesHierarchy?.Dispose();
			_menuNodesHierarchy = null;
		}
		#endregion IDisposable

		private void FillMenusHierarchy()
		{
			string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab");
			int prefabsCount = prefabGuids.Length;
			for (int i = 0; i < prefabsCount; i++)
			{
				string prefabGuid = prefabGuids[i];
				string path = AssetDatabase.GUIDToAssetPath(prefabGuid);
				GameObject menuNodePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

				if (menuNodePrefab != null && menuNodePrefab.GetComponent<MenuUI>() != null)
				{
					_menuNodesHierarchy.AddMenuPrefab(path);
				}
			}
		}
		#endregion Methods
	}
}