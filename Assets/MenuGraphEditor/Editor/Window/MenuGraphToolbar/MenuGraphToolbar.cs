namespace MenuGraph.Editor
{
	using System;
	using UnityEditor.UIElements;
	using UnityEngine.UIElements;
	using VisualElementHelper;

	[UxmlElement]
	internal sealed partial class MenuGraphToolbar : Toolbar, IDisposable
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
		public void Dispose()
		{
			if (_thumbnailWidthSlider != null)
			{
				_thumbnailWidthSlider.UnregisterValueChangedCallback(OnThumbnailWidthSliderValueChanged);
				_thumbnailWidthSlider = null;
			}
		}

		private void OnThumbnailWidthSliderValueChanged(ChangeEvent<float> changeEvent)
		{
			MenuGraphEditorPrefs.SaveThumbnailWidth(changeEvent.newValue);
		}
		#endregion Methods
	}
}