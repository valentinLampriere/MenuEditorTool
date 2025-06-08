namespace MenuGraph.Editor
{
	using UnityEditor;
	using UnityEngine;
	using UnityEngine.UIElements;

	internal sealed class MenuNodeHierarchyFoldout : Foldout
	{
		#region Fields
		private Image _foldoutIcon = null;
		private Toggle _foldoutToggle = null;

		private Texture _closedFoldoutImage = null;
		private Texture _openedFoldoutImage = null;
		#endregion Fields

		#region Constructors
		internal MenuNodeHierarchyFoldout(string text)
		{
			this.text = text;
			AddToClassList("unity-foldout");

			_foldoutToggle = this.Q<Toggle>();
			_foldoutIcon = CreateIcon();

			_foldoutToggle.style.marginBottom = 0;
			_foldoutToggle.style.marginTop = 0;

			_foldoutToggle.RegisterValueChangedCallback(OnToggleClicked);
		}
		#endregion Constructors

		#region Methods
		private Image CreateIcon()
		{
			Image icon = new Image();

			_closedFoldoutImage = EditorGUIUtility.IconContent("d_Folder Icon").image;
			_openedFoldoutImage = EditorGUIUtility.IconContent("d_FolderOpened Icon").image;

			icon.image = _foldoutToggle.value == true ? _openedFoldoutImage : _closedFoldoutImage;

			icon.scaleMode = ScaleMode.ScaleToFit;

			icon.style.width = 16;
			icon.style.height = 16;
			icon.style.left = -4;
			icon.style.flexShrink = 0;

			this.Q<Toggle>().ElementAt(0).Insert(1, icon);

			return icon;
		}

		private void OnToggleClicked(ChangeEvent<bool> changeEvent)
		{
			_foldoutIcon.image = changeEvent.newValue == true ? _openedFoldoutImage : _closedFoldoutImage;
		}
		#endregion Methods
	}
}