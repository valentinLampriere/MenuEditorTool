namespace MenuGraph.Editor
{
	using System;
	using UnityEditor;
	using UnityEngine;
	using UnityEngine.UIElements;

	internal sealed class MenuNodeThumbnailImage : VisualElement, IDisposable
	{
		#region Constants
		private const float THUMBNAIL_IMAGE_RATIO = 16.0f / 9.0f;
		private const float THUMBNAIL_IMAGE_INVERSED_RATIO = 1.0f / THUMBNAIL_IMAGE_RATIO;
		#endregion Constants

		#region Fields
		private Canvas _canvas = null;

		private Image _thumbnailImage = null;
		#endregion Fields

		#region Constructors
		internal MenuNodeThumbnailImage(Canvas canvas)
		{
			_canvas = canvas;

			_thumbnailImage = new Image();
			ApplyStyle(_thumbnailImage);
			ApplyWidth(MenuGraphEditorPrefs.GetSavedThumbnailWidth());
			TakeSnapshot();
			Add(_thumbnailImage);

			MenuGraphEditorPrefs.ThumbnailWidthChanged += OnThumbnailWidthChanged;
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
		}

		~MenuNodeThumbnailImage()
		{
			// Make sure to unregister from static event.
			MenuGraphEditorPrefs.ThumbnailWidthChanged -= OnThumbnailWidthChanged;
			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
		}
		#endregion Constructors

		#region Methods
		public void Dispose()
		{
			_canvas = null;
			_thumbnailImage = null;

			MenuGraphEditorPrefs.ThumbnailWidthChanged -= OnThumbnailWidthChanged;
			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
		}

		internal float ComputeHeight(float width)
		{
			return width * THUMBNAIL_IMAGE_INVERSED_RATIO;
		}

		private void TakeSnapshot()
		{
			CanvasSnapshotMaker canvasSnapshotMaker = new CanvasSnapshotMaker(_canvas);
			Texture2D texture = canvasSnapshotMaker.TakeSnapshot();
			canvasSnapshotMaker.Dispose();

			_thumbnailImage.image = texture;
		}

		private void ApplyStyle(Image thumbnailElement)
		{
			thumbnailElement.scaleMode = ScaleMode.ScaleAndCrop;
		}

		private void ApplyWidth(float width)
		{
			style.maxWidth = width;
			style.maxHeight = ComputeHeight(width);
		}

		private void OnThumbnailWidthChanged(float newThumbnailWidth)
		{
			ApplyWidth(newThumbnailWidth);
		}

		private void OnPlayModeStateChanged(PlayModeStateChange playModeStateChange)
		{
			if (playModeStateChange == PlayModeStateChange.EnteredEditMode)
			{
				TakeSnapshot();
			}
		}
		#endregion Methods
	}
}