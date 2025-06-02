namespace MenuGraph
{
	using UnityEngine;

	public sealed partial class MenuNode : ScriptableObject
	{
		#region Fields
		[SerializeField] private MenuUI _targetMenu = null;
		[SerializeField] private MenuNode _parent = null;
		#endregion Fields

		#region Properties
		// TODO: Instead of adding a setter, maybe it's possible to use Binding to bind the field to a UI element?
		public MenuUI TargetMenu { get { return _targetMenu; } set { _targetMenu = value; } }
		#endregion Properties
	}
}