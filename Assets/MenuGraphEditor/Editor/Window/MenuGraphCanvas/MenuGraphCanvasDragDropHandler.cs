namespace MenuGraph.Editor
{
	using System;
	using UnityEditor;
	using UnityEngine;
	using UnityEngine.UIElements;
	using Object = UnityEngine.Object;

	internal sealed class MenuGraphCanvasDragDropHandler : IDisposable
	{
		#region Delegates
		internal delegate void MenuNodeDropped(MenuUI menu, DragPerformEvent dragPerformEvent);
		#endregion Delegates

		#region Fields
		private VisualElement _target = null;
		private MenuNodeDropped _onMenuNodeDropped = null;
		#endregion Fields

		#region Constructors
		internal MenuGraphCanvasDragDropHandler(
			VisualElement target,
			MenuNodeDropped onMenuNodeDropped = null)
		{
			_target = target;
			_onMenuNodeDropped = onMenuNodeDropped;

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
				_target.UnregisterCallback<DragUpdatedEvent>(OnDragUpdated);
				_target.UnregisterCallback<DragPerformEvent>(OnDragPerformed);
				_target = null;
			}
		}
		#endregion Lifecycle

		#region Drag Drop Callbacks
		private void OnDragUpdated(DragUpdatedEvent dragUpdatedEvent)
		{
			bool isDraggingMenuUI = false;
			Object[] draggedObjects = DragAndDrop.objectReferences;
			foreach (Object draggedObject in draggedObjects)
			{
				if (draggedObject is MenuUI menuUI)
				{
					isDraggingMenuUI = true;
					break;
				}

				if (draggedObject is GameObject draggedGameObject && draggedGameObject.GetComponent<MenuUI>() != null)
				{
					isDraggingMenuUI = true;
					break;
				}
			}

			if (isDraggingMenuUI == true)
			{
				DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
				dragUpdatedEvent.StopPropagation();
			}
		}

		private void OnDragPerformed(DragPerformEvent dragPerformEvent)
		{
			Object[] droppedObjects = DragAndDrop.objectReferences;
			foreach (Object droppedObject in droppedObjects)
			{
				if (droppedObject is MenuUI menuUI ||
					droppedObject is GameObject droppedGameObject && droppedGameObject.TryGetComponent(out menuUI) == true)
				{
					_onMenuNodeDropped?.Invoke(menuUI, dragPerformEvent);
				}
			}

			DragAndDrop.AcceptDrag();
			dragPerformEvent.StopPropagation();
		}
		#endregion Drag Drop Callbacks
		#endregion Methods
	}
}