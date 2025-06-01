namespace MenuGraph
{
	using UnityEngine;

	public sealed class MenuNode : ScriptableObject
	{
		#region Fields
		[SerializeField] private MenuUI _targetMenu = null;
		// TODO: Move this in an editor only script.
		[SerializeField] private Vector2 _editorPosition = Vector2.zero;
		#endregion Fields

		#region Properties
		// TODO: Instead of adding a setter, maybe it's possible to use Binding to bind the field to a UI element?
		public MenuUI TargetMenu { get { return _targetMenu; } set { _targetMenu = value; } }
		public Vector2 EditorPosition { get { return _editorPosition; } }
		#endregion Properties
	}
}