namespace MenuGraph
{
	using System.Collections.Generic;
	using UnityEngine;

	[CreateAssetMenu(fileName = "MenuGraph", menuName = "MenuGraph")]
	public sealed partial class MenuGraph : ScriptableObject
	{
		#region Fields
		// TODO : A potential improvement is :
		// - Do no serialize a "root" node, but provide a method to retrieve the parent of all nodes.
		// - This require the graph to have a single parent for every nodes.
		// - Needs to warn the user who's editing the MenuGraph in the MenuGraphWindow.
		[SerializeField] private MenuNode _rootMenuNode = null;
		[SerializeField] private List<MenuNode> _menuNodes = null;
		#endregion Fields

		#region Properties
		public MenuNode RootMenuNode { get { return _rootMenuNode; } }
		public IReadOnlyList<MenuNode> MenuNodes { get { return _menuNodes; } }
		#endregion Properties
	}
}