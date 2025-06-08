namespace MenuGraph.Editor
{
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEditor.Experimental.GraphView;
	using UnityEngine;
	using UnityEngine.UIElements;

	internal sealed class MenuNodeView : Node
	{
		#region Constants
		private const string NODE_CONTENT_ELEMENT_ID = "contents";
		#endregion Constants

		#region Fields
		private MenuNode _menuNode = null;

		private Port _inputPort = null;
		private List<Port> _outputPorts = null;

		private MenuNodeThumbnailImage _menuNodeThumbnailImage = null;
		#endregion Fields

		#region Properties
		internal MenuNode MenuNode { get { return _menuNode; } }
		internal Port InputPort { get { return _inputPort; } }
		internal IReadOnlyList<Port> OutputPorts { get { return _outputPorts; } }
		#endregion Properties

		#region Constructors
		internal MenuNodeView(MenuNode menuNode)
		{
			_menuNode = menuNode;

			title = menuNode.name;

			AddSnapshot();
			CreateInputPort();
			CreateOutputPorts();

			MenuGraphEditorPrefs.ThumbnailWidthChanged += OnThumbnailWidthChanged;
		}
		#endregion Constructors

		#region Methods
		#region Node
		public override Rect GetPosition()
		{
			Rect rect = base.GetPosition();

			float width = MenuGraphEditorPrefs.GetSavedThumbnailWidth();
			float height = _menuNodeThumbnailImage.ComputeHeight(width);

			rect.x += width * 0.5f;
			rect.y += height * 0.5f;

			return rect;
		}

		public override void SetPosition(Rect newPos)
		{
			base.SetPosition(newPos);

			float width = MenuGraphEditorPrefs.GetSavedThumbnailWidth();
			float height = _menuNodeThumbnailImage.ComputeHeight(width);

			style.left = newPos.x - width * 0.5f;
			style.top = newPos.y - height * 0.5f;

			_menuNode.EditorPosition = newPos.position;
		}

		public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
		{
			evt.menu.AppendAction("Select in Project", OnSelectMenuInProject);
		}

		protected override void ToggleCollapse()
		{
			base.ToggleCollapse();

			_menuNodeThumbnailImage.style.display = expanded == true ? DisplayStyle.Flex : DisplayStyle.None;
		}
		#endregion Node

		private void AddSnapshot()
		{
			Canvas canvas = _menuNode.TargetMenu.GetComponent<Canvas>();
			CanvasSnapshotMaker canvasSnapshotMaker = new CanvasSnapshotMaker(canvas);
			Texture2D texture = canvasSnapshotMaker.TakeSnapshot();

			VisualElement content = this.Q(NODE_CONTENT_ELEMENT_ID);
			_menuNodeThumbnailImage = new MenuNodeThumbnailImage(texture);
			content.Insert(0, _menuNodeThumbnailImage);
		}

		private void CreateInputPort()
		{
			_inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(Port));
			_inputPort.portName = string.Empty;
			inputContainer.Add(_inputPort);
		}

		private void CreateOutputPorts()
		{
			_outputPorts = new List<Port>();

			foreach (MenuUI.MenuNodeAction menuAction in _menuNode.TargetMenu.MenuActions)
			{
				if (menuAction.IsValid() == false)
				{
					continue;
				}

				Port outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(Port));
				outputPort.portName = menuAction.EditorName;
				outputContainer.Add(outputPort);
				_outputPorts.Add(outputPort);
			}
		}

		private void OnThumbnailWidthChanged(float newThumbnailWidth)
		{
			Rect rect = GetPosition();
			SetPosition(new Rect(_menuNode.EditorPosition.x, _menuNode.EditorPosition.y, rect.width, rect.height));
		}

		private void OnSelectMenuInProject(DropdownMenuAction menuAction)
		{
			Selection.activeObject = _menuNode.TargetMenu.gameObject;
		}
		#endregion Methods
	}
}