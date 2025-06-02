namespace MenuGraph
{
	using UnityEngine;

	public sealed partial class MenuNode
	{
		#region Fields
		[SerializeField] private Vector2 _editorPosition = Vector2.zero;
		#endregion Fields

		#region Properties
		public Vector2 EditorPosition { get { return _editorPosition; } set { _editorPosition = value; } }
		#endregion Properties
	}
}