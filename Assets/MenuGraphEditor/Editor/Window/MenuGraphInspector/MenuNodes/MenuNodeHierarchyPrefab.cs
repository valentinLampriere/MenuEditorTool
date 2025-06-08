namespace MenuGraph.Editor
{
	using System;
	using System.IO;
	using UnityEditor;
	using UnityEngine;
	using UnityEngine.UIElements;
	using VisualElementHelper;

	internal sealed class MenuNodeHierarchyPrefab : VisualElement, IDisposable
	{
		#region Constants
		private const string PREFAB_ICON_UXML_ID = "PrefabIcon";
		private const string PREFAB_NAME_UXML_ID = "PrefabName";

		private const float REQUIRED_MOUSE_OFFSET_FOR_DRAG_SQR = 10.0f;
		#endregion Constants

		#region Fields
		private string _prefabPath = string.Empty;
		private MenuUI _associatedPrefab = null;

		private Vector2 _cursorPositionOnMouseDown = Vector2.zero;
		private bool _isMouseDownOnThis = false;
		#endregion Fields

		#region Constructors
		internal MenuNodeHierarchyPrefab(string prefabPath)
		{
			_prefabPath = prefabPath;
			_associatedPrefab = AssetDatabase.LoadAssetAtPath<MenuUI>(_prefabPath);

			this.LoadUXML();

			Image icon = this.Q<Image>(PREFAB_ICON_UXML_ID);
			Label label = this.Q<Label>(PREFAB_NAME_UXML_ID);

			SetIcon(icon);
			SetName(label);

			//this.AddToClassList("draggable-entry");
			RegisterCallback<MouseDownEvent>(OnMouseDown);
			RegisterCallback<MouseMoveEvent>(OnMouseMove);
			RegisterCallback<MouseUpEvent>(OnMouseUp);
		}
		#endregion Constructors

		#region Methods
		public void Dispose()
		{
			_associatedPrefab = null;

			UnregisterCallback<MouseDownEvent>(OnMouseDown);
			UnregisterCallback<MouseMoveEvent>(OnMouseMove);
			UnregisterCallback<MouseUpEvent>(OnMouseUp);
		}

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

			//Texture assetTexture = EditorGUIUtility.GetIconForObject(_associatedPrefab);
			//Texture prefabTexture = EditorGUIUtility.IconContent("d_Prefab Icon").image;
			Texture assetTexture = AssetDatabase.GetCachedIcon(AssetDatabase.GetAssetPath(_associatedPrefab));

			icon.image = assetTexture;
		}

		private void OnMouseDown(MouseDownEvent mouseDownEvent)
		{
			if (mouseDownEvent.button != 0) // Left click
			{
				return;
			}

			_cursorPositionOnMouseDown = mouseDownEvent.mousePosition;
			_isMouseDownOnThis = true;
		}

		private void OnMouseMove(MouseMoveEvent mouseMoveEvent)
		{
			if (mouseMoveEvent.button != 0 || _isMouseDownOnThis == false)
			{
				return;
			}

			Vector2 mousePosition = mouseMoveEvent.mousePosition;
			Vector2 diffPosition = mousePosition - _cursorPositionOnMouseDown;

			if (diffPosition.sqrMagnitude >= REQUIRED_MOUSE_OFFSET_FOR_DRAG_SQR)
			{
				DragAndDrop.PrepareStartDrag();
				DragAndDrop.objectReferences = new[] { _associatedPrefab };
				DragAndDrop.StartDrag("Dragging menu asset");
				_isMouseDownOnThis = false;
			}
		}

		private void OnMouseUp(MouseUpEvent mouseUpEvent)
		{
			if (_isMouseDownOnThis == true && mouseUpEvent.button == 0) // Left click
			{
				Selection.activeObject = _associatedPrefab;
			}

			_isMouseDownOnThis = false;
		}
		#endregion Methods
	}
}