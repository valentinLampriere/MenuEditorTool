namespace MenuGraph.Editor
{
	using UnityEditor.Experimental.GraphView;

	internal sealed class MenuNodeView : Node
	{
		#region Fields
		private MenuNode _menuNode = null;
		#endregion Fields

		#region Properties
		internal MenuNode MenuNode { get { return _menuNode; } }
		#endregion Properties

		#region Constructors
		internal MenuNodeView(MenuNode menuNode)
		{
			_menuNode = menuNode;

			this.title = menuNode.name;
		}
		#endregion Constructors

		#region Methods

		#endregion Methods
	}
}