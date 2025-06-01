namespace MenuGraph.Editor
{
	using UnityEngine.UIElements;

	internal sealed class MenuNodesHierarchy : VisualElement
	{
		#region Constants
		private const char SEPARATOR = '/';
		#endregion Constants

		#region Methods
		internal void AddMenuPrefab(string menuNodePrefabPath)
		{
			AddPathInHierarchyTree(menuNodePrefabPath, this, 0);
		}

		private void AddPathInHierarchyTree(string path, VisualElement currentElement, int depth)
		{
			string[] splittedPath = path.Split(SEPARATOR);
			if (depth >= splittedPath.Length)
			{
				return;
			}

			string currentFolder = splittedPath[depth];

			if (TryGetTreeNodeChildWithValue(currentElement, currentFolder, out VisualElement childElement) == false)
			{
				if (depth < splittedPath.Length - 1)
				{
					childElement = new MenuNodeHierarchyFoldout(currentFolder);
				}
				else
				{
					childElement = new MenuNodeHierarchyPrefab(path);
				}

				childElement.name = currentFolder;
				currentElement.Add(childElement);
			}

			AddPathInHierarchyTree(path, childElement, depth + 1);
		}

		private bool TryGetTreeNodeChildWithValue(VisualElement element, string value, out VisualElement childElement)
		{
			int childrenCount = element.childCount;
			for (int i = 0; i < childrenCount; i++)
			{
				childElement = element.ElementAt(i);
				if (childElement.name == value)
				{
					return true;
				}
			}

			childElement = null;
			return false;
		}
		#endregion Methods
	}
}