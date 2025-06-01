namespace MenuGraph
{
	using System.Collections.Generic;
	using UnityEngine;

	[CreateAssetMenu(fileName = "MenuGraph", menuName = "MenuGraph")]
	public sealed partial class MenuGraph : ScriptableObject
	{
		#region Fields
		[SerializeField] private List<MenuNode> _menuNodes = null;
		#endregion Fields

		#region Properties
		public IReadOnlyList<MenuNode> MenuNodes { get { return _menuNodes; } }
		#endregion Properties
	}
}