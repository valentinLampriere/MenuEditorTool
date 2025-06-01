namespace MenuGraph.Editor
{
	using UnityEditor.Experimental.GraphView;

	internal sealed class MenuNodeView : Node
	{
		#region Constructors
		internal MenuNodeView(MenuNode menuNode)
		{
			this.title = menuNode.name;
		}
		#endregion Constructors

		#region Methods

		#endregion Methods
	}
}