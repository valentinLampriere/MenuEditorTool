namespace MenuGraph.Editor
{
	using UnityEditor;
	using UnityEngine;

	internal class CanvasSnapshotMaker
	{
		#region Fields
		private Canvas _canvas = null;
		#endregion Fields

		#region Constructors
		internal CanvasSnapshotMaker(Canvas canvas)
		{
			_canvas = canvas;
		}
		#endregion Constructors

		#region Methods
		#region APIs
		internal Texture2D TakeSnapshot()
		{
			GameObject root = new GameObject();
			GameObject canvasGameObjectCopy = GameObject.Instantiate(_canvas.gameObject, root.transform);
			Canvas canvasCopy = canvasGameObjectCopy.GetComponent<Canvas>();

			Rect canvasDimensions = GetCanvasDimension(canvasCopy);
			Camera camera = CreateCamera(canvasCopy);
			SetupCanvas(canvasCopy, camera);
			RenderTexture renderTexture = CreateRenderTexture(canvasDimensions);
			camera.targetTexture = renderTexture;
			camera.Render();

			Texture2D texture = ReadPixels(canvasDimensions, camera);

			GameObject.DestroyImmediate(camera.gameObject);
			GameObject.DestroyImmediate(root);
			GameObject.DestroyImmediate(renderTexture);
			AssetDatabase.Refresh();

			return texture;
		}
		#endregion APIs

		#region Privates
		private Camera CreateCamera(Canvas canvas)
		{
			GameObject cameraGameObject = new GameObject();
			cameraGameObject.transform.SetParent(canvas.transform.parent ?? canvas.transform, false);

			Camera camera = cameraGameObject.AddComponent<Camera>();
			camera.orthographic = true;
			camera.clearFlags = CameraClearFlags.SolidColor;
			camera.backgroundColor = Color.clear;
			//camera.cullingMask = LayerMask.GetMask("UI");

			return camera;
		}

		private void SetupCanvas(Canvas canvas, Camera camera)
		{
			canvas.renderMode = RenderMode.ScreenSpaceCamera;
			canvas.worldCamera = camera;
			canvas.planeDistance = camera.nearClipPlane;
		}

		private RenderTexture CreateRenderTexture(Rect canvasDimension)
		{
			int width = Mathf.FloorToInt(canvasDimension.width);
			int height = Mathf.FloorToInt(canvasDimension.height);

			RenderTexture renderTexture = new RenderTexture(width, height, 24); // 24?
			return renderTexture;
		}

		private Texture2D ReadPixels(Rect canvasDimension, Camera camera)
		{
			RenderTexture activeRenderTexture = RenderTexture.active;
			RenderTexture.active = camera.targetTexture;

			int width = Mathf.FloorToInt(canvasDimension.width);
			int height = Mathf.FloorToInt(canvasDimension.height);

			Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGBA32, false);
			screenshot.ReadPixels(new Rect(0, 0, canvasDimension.width, canvasDimension.height), 0, 0);
			screenshot.Apply();

			RenderTexture.active = activeRenderTexture;
			return screenshot;
		}

		private Rect GetCanvasDimension(Canvas canvas)
		{
			RectTransform uiElement = canvas.transform as RectTransform;

			Vector3[] worldCorners = new Vector3[4];
			uiElement.GetWorldCorners(worldCorners);

			Vector3 startPoint = worldCorners[0];
			Vector3 endPoint = worldCorners[2];

			float width = endPoint.x - startPoint.x;
			float height = endPoint.y - startPoint.y;
			Rect result = new Rect(startPoint.x, Screen.height - endPoint.y, width, height);
			return result;
		}
		#endregion Privates
		#endregion Methods
	}
}
