namespace MenuGraph.Editor
{
	using UnityEngine;
	using UnityEngine.UIElements;

	internal sealed class MenuNodeThumbnailImage : VisualElement
	{
		#region Constants
		private const float THUMBNAIL_IMAGE_RATIO = 16.0f / 9.0f;
		#endregion Constants

		#region Constructors
		internal MenuNodeThumbnailImage(Texture2D thumbnail)
		{
			Image thumbnailElement = CreateImageElement(thumbnail);
			ApplyStyle(thumbnailElement);
			ApplyWidth(MenuGraphEditorPrefs.GetSavedThumbnailWidth());
			Add(thumbnailElement);

			MenuGraphEditorPrefs.ThumbnailWidthChanged += OnThumbnailWidthChanged;
		}
		#endregion Constructors

		#region Methods
		private Image CreateImageElement(Texture2D thumbnail)
		{
			Image thumbnailElement = new Image();

			thumbnailElement.image = thumbnail;

			return thumbnailElement;
		}

		private void ApplyStyle(Image thumbnailElement)
		{
			thumbnailElement.scaleMode = ScaleMode.ScaleAndCrop;
		}

		private void ApplyWidth(float width)
		{
			style.maxWidth = width;
			style.maxHeight = width * (1.0f / THUMBNAIL_IMAGE_RATIO);
		}

		private void OnThumbnailWidthChanged(float newThumbnailWidth)
		{
			ApplyWidth(newThumbnailWidth);
		}
		#endregion Methods
	}
}