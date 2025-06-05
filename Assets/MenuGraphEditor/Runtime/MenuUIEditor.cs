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
		[SerializeField] private Texture2D _thumbnailTexture = null;
		#endregion Fields

		#region Properties
		//public Vector2 EditorPosition { get { return _editorPosition; } }
		public Texture2D EditorIcon { get { return _editorIcon; } }
		public Texture2D ThumbnailTexture { get { return _thumbnailTexture; } }
		#endregion Properties
	}
}
#endif // UNITY_EDITOR