namespace MenuGraph.Editor
{
	using System.IO;
	using UnityEditor;
	using UnityEngine;
	using UnityEngine.UIElements;
	using VisualElementHelper;

	internal sealed class MenuNodeHierarchyPrefab : VisualElement
	{
		#region Constants
		private const string PREFAB_ICON_UXML_ID = "PrefabIcon";
		private const string PREFAB_NAME_UXML_ID = "PrefabName";
		#endregion Constants

		#region Fields
		private string _prefabPath = string.Empty;
		private MenuNode _associatedPrefab = null;
		#endregion Fields

		#region Constructors
		internal MenuNodeHierarchyPrefab(string prefabPath)
		{
			_prefabPath = prefabPath;
			_associatedPrefab = AssetDatabase.LoadAssetAtPath<MenuNode>(_prefabPath);

			this.LoadUXML();

			Image icon = this.Q<Image>(PREFAB_ICON_UXML_ID);
			Label label = this.Q<Label>(PREFAB_NAME_UXML_ID);

			SetIcon(icon);
			SetName(label);

			//this.AddToClassList("draggable-entry");
			RegisterCallback<MouseDownEvent>(OnMouseDown);
		}
		#endregion Constructors

		#region Methods
		private void SetName(Label label)
		{
			string name = Path.GetFileNameWithoutExtension(_prefabPath);
			label.text = name;
		}

		private void SetIcon(Image icon)
		{
			if (_associatedPrefab.EditorIcon != null)
			{
				icon.image = _associatedPrefab.EditorIcon;
				return;
			}

			Texture prefabTexture = EditorGUIUtility.IconContent("d_Prefab Icon").image;
			icon.image = prefabTexture;
		}

		private void OnMouseDown(MouseDownEvent mouseDownEvent)
		{
			if (mouseDownEvent.button == 0) // Left click
			{
				DragAndDrop.PrepareStartDrag();
				DragAndDrop.objectReferences = new[] { _associatedPrefab };
				DragAndDrop.StartDrag("Dragging menu asset");
			}
		}
		#endregion Methods
	}
}