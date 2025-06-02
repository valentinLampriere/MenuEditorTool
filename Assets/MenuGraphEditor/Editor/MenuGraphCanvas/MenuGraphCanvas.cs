namespace MenuGraph.Editor
{
	using System.Collections.Generic;
	using UnityEditor;
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
		private GraphViewChangesHandler _graphViewChangesHandler = null;
		#endregion Fields

		#region Constructors
		public MenuGraphCanvas()
		{
			Insert(0, new GridBackground());
			this.LoadUSS();

			_dragDropHandler = new MenuGraphCanvasDragDropHandler(this, OnMenuNodeDropped);
			_graphViewChangesHandler = new GraphViewChangesHandler(this);
			_graphViewChangesHandler.GraphElementRemoved += OnGraphElementRemoved;
			_graphViewChangesHandler.EdgeCreated += OnEdgeCreated;

			this.AddManipulator(new ContentZoomer());
			this.AddManipulator(new ContentDragger());
			this.AddManipulator(new SelectionDragger());
			this.AddManipulator(new RectangleSelector());
		}

		~MenuGraphCanvas()
		{
			_dragDropHandler?.Dispose();
			_dragDropHandler = null;

			if (_graphViewChangesHandler != null)
			{
				_graphViewChangesHandler.GraphElementRemoved -= OnGraphElementRemoved;
				_graphViewChangesHandler.EdgeCreated -= OnEdgeCreated;

				_graphViewChangesHandler?.Dispose();
				_graphViewChangesHandler = null;
			}
		}
		#endregion Constructors

		#region Methods
		#region GraphView
		public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
		{
			List<Port> compatiblePorts = new List<Port>();
			foreach (Port port in ports)
			{
				if (port.direction != startPort.direction && startPort.node != port.node)
				{
					compatiblePorts.Add(port);
				}
			}

			return compatiblePorts;
		}
		#endregion GraphView

		internal void SetMenuGraph(MenuGraph menuGraph)
		{
			_targetMenuGraph = menuGraph;

			_graphViewChangesHandler.GraphElementRemoved -= OnGraphElementRemoved;
			DeleteElements(graphElements);
			_graphViewChangesHandler.GraphElementRemoved += OnGraphElementRemoved;

			foreach (MenuNode menuNode in menuGraph.MenuNodes)
			{
				MenuNodeView menuNodeView = new MenuNodeView(menuNode);
				Rect rect = menuNodeView.GetPosition();
				menuNodeView.SetPosition(new Rect(menuNode.EditorPosition.x, menuNode.EditorPosition.y, rect.width, rect.height));
				AddElement(menuNodeView);
			}
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

			Vector2 worldMousePosition = dragPerformEvent.mousePosition;
			Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);
			Vector2 nodePosition = localMousePosition;

			// Create the ScriptableObject asset.
			MenuNode newMenuNode = ScriptableObject.CreateInstance<MenuNode>();
			newMenuNode.name = menu.name;
			newMenuNode.TargetMenu = menu;
			newMenuNode.EditorPosition = nodePosition;
			_targetMenuGraph.AddMenuNode(newMenuNode);
			AssetDatabase.AddObjectToAsset(newMenuNode, _targetMenuGraph);

			// Create the node in the canvas.
			MenuNodeView menuNodeView = new MenuNodeView(newMenuNode);
			Rect rect = menuNodeView.GetPosition();
			menuNodeView.SetPosition(new Rect(nodePosition.x, nodePosition.y, rect.width, rect.height));
			AddElement(menuNodeView);

			AssetDatabase.SaveAssets();
		}

		private void OnGraphElementRemoved(GraphElement removedGraphElement)
		{
			if (removedGraphElement is MenuNodeView menuNodeView)
			{
				_targetMenuGraph.DeleteMenuNode(menuNodeView.MenuNode);
			}
		}

		private void OnEdgeCreated(Edge edgeCreated)
		{

		}
		#endregion Methods
	}
}