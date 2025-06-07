#if UNITY_EDITOR
namespace MenuGraph
{
	using UnityEngine;

	public sealed partial class MenuUI
	{
		#region Fields
		[Header("Editor Only")]
		[SerializeField] private Texture2D _editorIcon = null;
		[SerializeField] private Texture2D _editorThumbnail = null;
		#endregion Fields

		#region Properties
		public Texture2D EditorIcon { get { return _editorIcon; } }
		public Texture2D EditorThumbnail { get { return _editorThumbnail; } }
		#endregion Properties
	}
}
#endif // UNITY_EDITOR