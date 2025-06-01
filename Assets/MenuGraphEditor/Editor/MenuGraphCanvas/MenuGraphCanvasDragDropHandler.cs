namespace MenuGraph.Editor
{
	using System;
	using UnityEditor;
	using UnityEngine.UIElements;
	using Object = UnityEngine.Object;

	internal sealed class MenuGraphCanvasDragDropHandler : IDisposable
	{
		#region Delegates
		internal delegate void MenuNodeDraggedIn(MenuNode menuNode, DragEnterEvent dragEnterEvent);
		internal delegate void MenuNodeDropped(MenuNode menuNode, DragPerformEvent dragPerformEvent);
		#endregion Delegates

		#region Fields
		private VisualElement _target = null;
		private MenuNodeDraggedIn _onMenuNodeDraggedIn = null;
		private MenuNodeDropped _onMenuNodeDropped = null;
		#endregion Fields

		#region Constructors
		internal MenuGraphCanvasDragDropHandler(
			VisualElement target,
			MenuNodeDraggedIn onMenuNodeDraggedIn = null,
			MenuNodeDropped onMenuNodeDropped = null)
		{
			_target = target;
			_onMenuNodeDraggedIn = onMenuNodeDraggedIn;
			_onMenuNodeDropped = onMenuNodeDropped;

			_target.RegisterCallback<DragEnterEvent>(OnDragEnter);
			_target.RegisterCallback<DragUpdatedEvent>(OnDragUpdated);
			_target.RegisterCallback<DragPerformEvent>(OnDragPerformed);
		}
		#endregion Constructors

		#region Methods
		#region Lifecycle
		public void Dispose()
		{
			if (_target != null)
			{
				_target.UnregisterCallback<DragEnterEvent>(OnDragEnter);
				_target.UnregisterCallback<DragUpdatedEvent>(OnDragUpdated);
				_target.UnregisterCallback<DragPerformEvent>(OnDragPerformed);
				_target = null;
			}
		}
		#endregion Lifecycle

		#region Drag Drop Callbacks
		private void OnDragEnter(DragEnterEvent dragEnterEvent)
		{
			Object[] droppedObjects = DragAndDrop.objectReferences;
			foreach (Object droppedObject in droppedObjects)
			{
				_onMenuNodeDraggedIn?.Invoke(droppedObject as MenuNode, dragEnterEvent);
			}
		}

		private void OnDragUpdated(DragUpdatedEvent dragUpdatedEvent)
		{
			DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
			dragUpdatedEvent.StopPropagation();
		}

		private void OnDragPerformed(DragPerformEvent dragPerformEvent)
		{
			Object[] droppedObjects = DragAndDrop.objectReferences;
			foreach (Object droppedObject in droppedObjects)
			{
				_onMenuNodeDropped?.Invoke(droppedObject as MenuNode, dragPerformEvent);
			}

			DragAndDrop.AcceptDrag();
			dragPerformEvent.StopPropagation();
		}
		#endregion Drag Drop Callbacks
		#endregion Methods
	}
}