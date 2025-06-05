namespace MenuGraph.Editor
{
	using UnityEngine;
	using UnityEngine.UIElements;

	internal sealed class MenuNodeThumbnailImage : VisualElement
	{
		#region Constants
		// TODO: Currently hardcoded, but it would be great to have a slider in the window to change the size of the thumbnail.
		private const float THUMBNAIL_IMAGE_MAX_WIDTH = 300.0f;
		private const float THUMBNAIL_IMAGE_MAX_HEIGHT = 168.75f;
		#endregion Constants

		#region Constructors
		internal MenuNodeThumbnailImage(Texture2D thumbnail)
		{
			Image thumbnailElement = CreateImageElement(thumbnail);
			ApplyStyle(thumbnailElement);
			Add(thumbnailElement);
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

			style.maxWidth = THUMBNAIL_IMAGE_MAX_WIDTH;
			style.maxHeight = THUMBNAIL_IMAGE_MAX_HEIGHT;
		}
		#endregion Methods
	}
}