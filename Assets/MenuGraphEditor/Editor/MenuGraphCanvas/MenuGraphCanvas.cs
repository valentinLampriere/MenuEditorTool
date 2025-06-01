namespace MenuGraph.Editor
{
	using UnityEditor.Experimental.GraphView;
	using UnityEngine;
	using UnityEngine.UIElements;
	using VisualElementHelper;

	[UxmlElement]
	internal sealed partial class MenuGraphCanvas : GraphView
	{
		#region Fields
		private MenuGraphCanvasDragDropHandler _dragDropHandler = null;
		#endregion Fields

		#region Constructors
		public MenuGraphCanvas()
		{
			Insert(0, new GridBackground());
			this.LoadUSS();

			_dragDropHandler = new MenuGraphCanvasDragDropHandler(this, onMenuNodeDropped : OnMenuNodeDropped);

			this.AddManipulator(new ContentZoomer());
			this.AddManipulator(new ContentDragger());
			this.AddManipulator(new SelectionDragger());
			this.AddManipulator(new RectangleSelector());
		}

		~MenuGraphCanvas()
		{
			_dragDropHandler?.Dispose();
			_dragDropHandler = null;
		}
		#endregion Constructors

		#region Methods
		internal void SetMenuGraph(MenuGraph menuGraph)
		{
			//DeleteElements(graphElements);

			//foreach (MenuNode menuNode in menuGraph.MenuNodes)
			//{
			//	MenuNodeView menuNodeView = new MenuNodeView(menuNode);
			//	AddElement(menuNodeView);
			//}
		}

		private void OnMenuNodeDropped(MenuNode menuNode, DragPerformEvent dragPerformEvent)
		{
			Vector2 nodePosition = dragPerformEvent.localMousePosition;

			MenuNodeView menuNodeView = new MenuNodeView(menuNode);
			Rect rect = menuNodeView.GetPosition();
			menuNodeView.SetPosition(new Rect(nodePosition.x, nodePosition.y, rect.width, rect.height));

			AddElement(menuNodeView);
		}
		#endregion Methods
	}
}