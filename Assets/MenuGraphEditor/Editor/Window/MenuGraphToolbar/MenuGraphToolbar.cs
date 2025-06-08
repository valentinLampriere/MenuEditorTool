namespace MenuGraph.Editor
{
	using UnityEditor.UIElements;
	using UnityEngine.UIElements;
	using VisualElementHelper;

	[UxmlElement]
	internal sealed partial class MenuGraphToolbar : Toolbar
	{
		#region Constants
		private const string THUMBNAIL_WIDTH_SLIDER_ID = "thumbnailWidthSlider";
		#endregion Constants

		#region Fields
		private Slider _thumbnailWidthSlider = null;
		#endregion Fields

		#region Constructors
		public MenuGraphToolbar()
		{
			this.LoadUXML();

			_thumbnailWidthSlider = this.Q<Slider>();
			_thumbnailWidthSlider.RegisterValueChangedCallback(OnThumbnailWidthSliderValueChanged);

			if (MenuGraphEditorPrefs.HasSavedThumbnailWidth() == false)
			{
				MenuGraphEditorPrefs.SaveThumbnailWidth(_thumbnailWidthSlider.value);
			}
			else
			{
				_thumbnailWidthSlider.value = MenuGraphEditorPrefs.GetSavedThumbnailWidth();
			}
		}
		#endregion Constructors

		#region Methods
		private void OnThumbnailWidthSliderValueChanged(ChangeEvent<float> changeEvent)
		{
			MenuGraphEditorPrefs.SaveThumbnailWidth(changeEvent.newValue);
		}
		#endregion Methods
	}
}