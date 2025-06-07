namespace MenuGraph.Editor
{
	using System;
	using UnityEditor;
	using UnityEngine;
	using UnityEngine.UIElements;

	[CustomEditor(typeof(MenuUI))]
	internal sealed class MenuUICustomEditor : Editor
	{
		#region Constants
		private const string EDITOR_THUMBNAIL_FIELD_NAME = "_editorThumbnail";
		#endregion Constants

		public override VisualElement CreateInspectorGUI()
		{
			// TODO : Might want to create a better UI.
			IMGUIContainer defaultImGui = new IMGUIContainer(() =>
			{
				DrawDefaultInspector();
			});

			VisualElement root = new VisualElement();

			Button snapshotButton = new Button(OnSnapshotButtonClicked);
			snapshotButton.text = "Take snapshot";

			root.Add(defaultImGui);
			root.Add(snapshotButton);

			return root;
		}

		private void OnSnapshotButtonClicked()
		{
			MenuUI menuUI = target as MenuUI;
			Canvas canvas = menuUI.GetComponent<Canvas>();
			CanvasSnapshotMaker canvasSnapshotMaker = new CanvasSnapshotMaker(canvas);
			Texture2D texture = canvasSnapshotMaker.TakeSnapshot();

			SerializedProperty editorThumbnailProperty = serializedObject.FindProperty(EDITOR_THUMBNAIL_FIELD_NAME);
			editorThumbnailProperty.objectReferenceValue = texture;
			editorThumbnailProperty.serializedObject.ApplyModifiedProperties();
		}
	}
}