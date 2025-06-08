namespace MenuGraph.Editor
{
	using System;
	using System.Collections.Generic;
	using UnityEngine.UIElements;

	internal sealed class MenuNodesHierarchy : VisualElement, IDisposable
	{
		#region Constants
		private const char SEPARATOR = '/';
		#endregion Constants

		#region Fields
		private List<IDisposable> _disposableChildren = null;
		#endregion Fields

		#region Constructors
		internal MenuNodesHierarchy()
		{
			_disposableChildren = new List<IDisposable>();
		}
		#endregion Constructors

		#region Methods
		public void Dispose()
		{
			if (_disposableChildren != null)
			{
				int disposableChildrenCount = _disposableChildren.Count;
				for (int i = 0; i < disposableChildrenCount; i++)
				{
					_disposableChildren[i]?.Dispose();
				}

				_disposableChildren.Clear();
				_disposableChildren = null;
			}
		}

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
				_disposableChildren.Add(childElement as IDisposable);
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