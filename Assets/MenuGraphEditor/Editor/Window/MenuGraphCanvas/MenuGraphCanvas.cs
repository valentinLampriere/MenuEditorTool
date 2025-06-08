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
		private Slider _widthSlider = null;

		private MenuGraphCanvasDragDropHandler _dragDropHandler = null;
		private GraphViewChangesHandler _graphViewChangesHandler = null;
		private SelectionWatcher _selectionWatcher = null;
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

			this.AddManipulator(new ContentZoomer() { minScale = 0.5f, maxScale = 1.5f});
			this.AddManipulator(new ContentDragger());
			this.AddManipulator(new SelectionDragger());
			this.AddManipulator(new RectangleSelector());

			_selectionWatcher = new SelectionWatcher();
			_selectionWatcher.Register<ObjectSelectedComponent, MenuNode>(OnMenuNodeSelected);
			_selectionWatcher.Register<ObjectDeselectedComponent, MenuNode>(OnMenuNodeDeselected);
		}

		// TODO : Handle Dispose
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

			if (_selectionWatcher != null)
			{
				_selectionWatcher.Unregister<ObjectSelectedComponent, MenuNode>(OnMenuNodeSelected);
				_selectionWatcher.Unregister<ObjectDeselectedComponent, MenuNode>(OnMenuNodeDeselected);
				_selectionWatcher.Dispose();
				_selectionWatcher = null;
			}

			if (_widthSlider != null)
			{
				_widthSlider.UnregisterValueChangedCallback(OnWidthSliderValueChanged);
				_widthSlider = null;
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

		internal void PopulateMenuGraph(MenuGraph menuGraph)
		{
			_targetMenuGraph = menuGraph;

			_graphViewChangesHandler.GraphElementRemoved -= OnGraphElementRemoved;
			DeleteElements(graphElements);
			_graphViewChangesHandler.GraphElementRemoved += OnGraphElementRemoved;

			int menuNodesCount = menuGraph.MenuNodes.Count;

			Dictionary<MenuNode, MenuNodeView> _menuNodeToViews = new Dictionary<MenuNode, MenuNodeView>();

			for (int i = 0; i < menuNodesCount; i++)
			{
				MenuNode menuNode = menuGraph.MenuNodes[i];
				MenuNodeView menuNodeView = new MenuNodeView(menuNode);
				Rect rect = menuNodeView.GetPosition();
				menuNodeView.SetPosition(new Rect(menuNode.EditorPosition.x, menuNode.EditorPosition.y, rect.width, rect.height));
				AddElement(menuNodeView);
				_menuNodeToViews.Add(menuNode, menuNodeView);
			}

			for (int i = 0; i < menuNodesCount; i++)
			{
				MenuNode parentNode = menuGraph.MenuNodes[i];
				int childrenCount = parentNode.Children.Count;

				for (int j = 0; j < childrenCount; j++)
				{
					MenuNode child = parentNode.Children[j];

					if (child == null)
					{
						continue;
					}

					if (_menuNodeToViews.TryGetValue(parentNode, out MenuNodeView parentView) == false ||
						_menuNodeToViews.TryGetValue(child, out MenuNodeView childView) == false)
					{
						Debug.LogError($"Failed to retrieve a {nameof(MenuNodeView)} from a {nameof(MenuNode)}" +
							$"(from {parentNode.name} or {child.name})");
						continue;
					}

					int parentNodeOutputPortsCount = parentView.OutputPorts.Count;

					Edge edge = parentView.OutputPorts[j].ConnectTo(childView.InputPort);
					AddElement(edge);
				}
			}
		}

		internal void SetWidthSlider(Slider widthSlider)
		{
			if (_widthSlider != null)
			{
				_widthSlider.UnregisterValueChangedCallback(OnWidthSliderValueChanged);
			}

			_widthSlider = widthSlider;
			_widthSlider.RegisterValueChangedCallback(OnWidthSliderValueChanged);
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

			if (removedGraphElement is Edge edge)
			{
				MenuNodeView parentView = edge.output.node as MenuNodeView;
				MenuNodeView childView = edge.input.node as MenuNodeView;

				childView.MenuNode.Parent = null;

				if (TryGetPortIndex(parentView, edge.output, out int childIndex) == false)
				{
					Debug.LogError($"Couldn't find the index of the port \"{edge.output.portName}\"" +
						$"from the {nameof(MenuNodeView)} {parentView.name}");
					return;
				}

				parentView.MenuNode.Children[childIndex] = null;
			}
		}

		private void OnEdgeCreated(Edge edgeCreated)
		{
			MenuNodeView parentView = edgeCreated.output.node as MenuNodeView;
			MenuNodeView childView = edgeCreated.input.node as MenuNodeView;

			if (TryGetPortIndex(parentView, edgeCreated.output, out int childIndex) == false)
			{
				Debug.LogError($"Couldn't find the index of the port \"{edgeCreated.output.portName}\"" +
					$"from the {nameof(MenuNodeView)} {parentView.name}");
				return;
			}

			childView.MenuNode.Parent = parentView.MenuNode;
			parentView.MenuNode.Children[childIndex] = childView.MenuNode;
		}

		private void OnWidthSliderValueChanged(ChangeEvent<float> changeEvent)
		{
			foreach (Node node in nodes)
			{
				MenuNodeView menuNodeView = node as MenuNodeView;
			}
		}

		/// <summary>
		/// Loop through the output ports of the given <paramref name="menuNodeView"/> and return the index of the port
		/// which match the given <paramref name="port"/>.
		/// </summary>
		private bool TryGetPortIndex(MenuNodeView menuNodeView, Port port, out int childIndex)
		{
			int parentOutputCount = menuNodeView.outputContainer.childCount;
			for (int i = 0; i < parentOutputCount; i++)
			{
				Port outputPort = menuNodeView.outputContainer.ElementAt(i) as Port;

				if (port == outputPort)
				{
					childIndex = i;
					return true;
				}
			}

			childIndex = -1;
			return false;
		}

		private void OnMenuNodeSelected(MenuNode menuNode)
		{
			foreach (Node node in nodes)
			{
				MenuNodeView menuNodeView = node as MenuNodeView;
				
				if (menuNodeView.MenuNode == menuNode)
				{
					AddToSelection(node);
				}
			}
		}

		private void OnMenuNodeDeselected(MenuNode menuNode)
		{
			foreach (Node node in nodes)
			{
				MenuNodeView menuNodeView = node as MenuNodeView;
				
				if (menuNodeView.MenuNode == menuNode)
				{
					RemoveFromSelection(node);
				}
			}
		}
		#endregion Methods
	}
}