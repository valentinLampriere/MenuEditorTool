#if UNITY_EDITOR
namespace MenuGraph
{
	using UnityEngine;

	public partial class MenuUI
	{
		#region Fields
		[Header("Editor Only")]
		[SerializeField] private Texture2D _editorIcon = null;
		#endregion Fields

		#region Properties
		public Texture2D EditorIcon { get { return _editorIcon; } }
		#endregion Properties
	}
}
#endif // UNITY_EDITOR