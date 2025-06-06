namespace MenuGraph.Editor
{
	using System.Collections.Generic;
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

			this.title = menuNode.name;

			AddSnapshot();
			CreateInputPort();
			CreateOutputPorts();
		}
		#endregion Constructors

		#region Methods
		public override void SetPosition(Rect newPos)
		{
			base.SetPosition(newPos);

			_menuNode.EditorPosition = newPos.position;
		}

		private void AddSnapshot()
		{
			VisualElement content = this.Q(NODE_CONTENT_ELEMENT_ID);
			content.Insert(0, new MenuNodeThumbnailImage(_menuNode.TargetMenu.ThumbnailTexture));
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
		#endregion Methods
	}
}