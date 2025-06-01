namespace MenuGraph.Editor
{
	using System;
	using UnityEditor;
	//using UnityEditor;
	using UnityEditor.Experimental.GraphView;
	using UnityEngine;
	using UnityEngine.UIElements;
	using VisualElementHelper;

	[UxmlElement]
	internal sealed partial class MenuGraphCanvas : GraphView
	{
		#region Fields
		private MenuGraph _targetMenuGraph = null;

		private MenuGraphCanvasDragDropHandler _dragDropHandler = null;
		#endregion Fields

		#region Constructors
		public MenuGraphCanvas()
		{
			Insert(0, new GridBackground());
			this.LoadUSS();

			_dragDropHandler = new MenuGraphCanvasDragDropHandler(this, onMenuNodeDropped : OnMenuNodeDropped);

			this.AddManipulator(new ContentZoomer());
			this.AddManipulator(new ContentDragger());
			this.AddManipulator(new SelectionDragger());
			this.AddManipulator(new RectangleSelector());
		}

		~MenuGraphCanvas()
		{
			_dragDropHandler?.Dispose();
			_dragDropHandler = null;
		}
		#endregion Constructors

		#region Methods
		internal void SetMenuGraph(MenuGraph menuGraph)
		{
			_targetMenuGraph = menuGraph;

			graphViewChanged -= OnGravViewChanged;
			DeleteElements(graphElements);
			graphViewChanged += OnGravViewChanged;

			foreach (MenuNode menuNode in menuGraph.MenuNodes)
			{
				MenuNodeView menuNodeView = new MenuNodeView(menuNode);
				AddElement(menuNodeView);
			}
		}

		private GraphViewChange OnGravViewChanged(GraphViewChange graphViewChange)
		{
			if (graphViewChange.elementsToRemove != null)
			{
				foreach (GraphElement elementToRemove in graphViewChange.elementsToRemove)
				{
					if (elementToRemove is MenuNodeView menuNodeView)
					{
						_targetMenuGraph.DeleteMenuNode(menuNodeView.MenuNode);
					}
				}
			}

			return graphViewChange;
		}

		private void OnMenuNodeDropped(MenuUI menu, DragPerformEvent dragPerformEvent)
		{
			if (_targetMenuGraph == null)
			{
				// TODO : Allow the user to add nodes even if a MenuGraph isn't selected.
				// Like adding GameObjects in the Scene/Hierarchy, even if no Scenes have been selected.
				// Ask the user to save as a new MenuGraph asset afterward.
				Debug.LogError($"A MenuGraph is required.");
				return;
			}

			// Create the ScriptableObject asset.
			MenuNode newMenuNode = ScriptableObject.CreateInstance<MenuNode>();
			newMenuNode.name = menu.name;
			newMenuNode.TargetMenu = menu;
			_targetMenuGraph.AddMenuNode(newMenuNode);
			AssetDatabase.AddObjectToAsset(newMenuNode, _targetMenuGraph);

			// Create the node in the canvas.
			Vector2 worldMousePosition = dragPerformEvent.mousePosition;
			Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);
			Vector2 nodePosition = localMousePosition;
			MenuNodeView menuNodeView = new MenuNodeView(newMenuNode);
			Rect rect = menuNodeView.GetPosition();
			menuNodeView.SetPosition(new Rect(nodePosition.x, nodePosition.y, rect.width, rect.height));

			AssetDatabase.SaveAssets();

			AddElement(menuNodeView);
		}
		#endregion Methods
	}
}