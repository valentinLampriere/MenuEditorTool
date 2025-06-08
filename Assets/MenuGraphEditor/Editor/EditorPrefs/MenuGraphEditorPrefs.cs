namespace MenuGraph.Editor
{
	using System;
	using UnityEditor;

	internal static class MenuGraphEditorPrefs
	{
		#region Constants
		private const string THUMBNAIL_WIDTH_EDITOR_PREFS_KEY = "MenuGraph.ThumbnailWidth";
		#endregion Constants

		#region Events
		private static Action<float> _thumbnailWidthChangedEvent = null;
		public static event Action<float> ThumbnailWidthChanged
		{
			add
			{
				_thumbnailWidthChangedEvent -= value;
				_thumbnailWidthChangedEvent += value;
			}
			remove
			{
				_thumbnailWidthChangedEvent -= value;
			}
		}
		#endregion Events

		#region Methods
		internal static void SaveThumbnailWidth(float thumbnailFloat)
		{
			float currentThumbnailWidth = EditorPrefs.GetFloat(THUMBNAIL_WIDTH_EDITOR_PREFS_KEY);
			if (currentThumbnailWidth == thumbnailFloat)
			{
				return;
			}

			EditorPrefs.SetFloat(THUMBNAIL_WIDTH_EDITOR_PREFS_KEY, thumbnailFloat);
			_thumbnailWidthChangedEvent?.Invoke(thumbnailFloat); 
		}

		internal static float GetSavedThumbnailWidth()
		{
			return EditorPrefs.GetFloat(THUMBNAIL_WIDTH_EDITOR_PREFS_KEY);
		}

		internal static bool HasSavedThumbnailWidth()
		{
			return EditorPrefs.HasKey(THUMBNAIL_WIDTH_EDITOR_PREFS_KEY);
		}
		#endregion Methods
	}
}