#if UNITY_EDITOR
namespace MenuGraph
{
	using UnityEngine;

	public sealed partial class MenuUI
	{
		#region Fields
		[Header("Editor Only")]
		//[SerializeField] private Vector2 _editorPosition = Vector2.zero;
		[SerializeField] private Texture2D _editorIcon = null;
		#endregion Fields

		#region Properties
		//public Vector2 EditorPosition { get { return _editorPosition; } }
		public Texture2D EditorIcon { get { return _editorIcon; } }
		#endregion Properties
	}
}
#endif // UNITY_EDITOR