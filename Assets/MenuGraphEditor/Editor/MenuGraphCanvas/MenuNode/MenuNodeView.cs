namespace MenuGraph.Editor
{
	using System.Collections.Generic;
	using UnityEditor.Experimental.GraphView;
	using UnityEngine;

	internal sealed class MenuNodeView : Node
	{
		#region Fields
		private MenuNode _menuNode = null;

		private Port _inputPort = null;
		private List<Port> _outputPort = null;
		#endregion Fields

		#region Properties
		internal MenuNode MenuNode { get { return _menuNode; } }
		#endregion Properties

		#region Constructors
		internal MenuNodeView(MenuNode menuNode)
		{
			_menuNode = menuNode;

			this.title = menuNode.name;

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

		private void CreateInputPort()
		{
			_inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(Port));
			_inputPort.portName = string.Empty;
			inputContainer.Add(_inputPort);
		}

		private void CreateOutputPorts()
		{
			_outputPort = new List<Port>();

			foreach (MenuUI.MenuNodeAction menuAction in _menuNode.TargetMenu.MenuActions)
			{
				if (menuAction.IsValid() == false)
				{
					continue;
				}

				Port outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(Port));
				outputPort.portName = menuAction.EditorName;
				outputContainer.Add(outputPort);
				_outputPort.Add(outputPort);
			}
		}
		#endregion Methods
	}
}