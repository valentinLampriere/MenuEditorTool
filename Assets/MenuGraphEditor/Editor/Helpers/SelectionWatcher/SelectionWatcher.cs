namespace MenuGraph.Editor
{
	using System;
	using UnityEditor;
	using UnityEngine;
	using Object = UnityEngine.Object;

	internal sealed class SelectionWatcher : IDisposable
	{
		#region Delegates
		internal delegate void OnObjectSelected<t_type>(t_type selectedObject) where t_type : Object;
		#endregion Delegates

		#region Fields
		private ISelectionWatcherComponent[] _selectionWatcherComponents = null;
		#endregion Fields

		#region Constructors
		internal SelectionWatcher()
		{
			_selectionWatcherComponents = new ISelectionWatcherComponent[]
			{
				new ObjectSelectedComponent(),
				new ObjectDeselectedComponent()
			};

			Selection.selectionChanged += OnSelectionChanged;
		}

		~SelectionWatcher()
		{
			// Unregister the event here as well because if Dispose is not called, this static event will never be unregister.
			Selection.selectionChanged -= OnSelectionChanged;
		}
		#endregion Constructors

		#region Methods
		#region IDisposable
		public void Dispose()
		{
			if (_selectionWatcherComponents != null)
			{
				int selectionWatcherComponentsCount = _selectionWatcherComponents.Length;
				for (int i = 0; i < selectionWatcherComponentsCount; i++)
				{
					_selectionWatcherComponents[i].Dispose();
				}

				_selectionWatcherComponents = null;
			}

			Selection.selectionChanged -= OnSelectionChanged;
		}
		#endregion IDisposable

		#region APIs
		internal bool Register<t_watcherComponent, t_type>(Action<t_type> callback) where t_watcherComponent : ISelectionWatcherComponent where t_type : Object
		{
			if (TryGetSelectionWatcher(out t_watcherComponent watcherComponent) == false)
			{
				Debug.LogWarning($"Can't register to a {nameof(ISelectionWatcherComponent)} of type {typeof(t_watcherComponent)}");
				return false;
			}

			watcherComponent.Register(callback);
			return true;
		}

		internal bool Unregister<t_watcherComponent, t_type>(Action<t_type> callback) where t_watcherComponent : ISelectionWatcherComponent where t_type : Object
		{
			if (TryGetSelectionWatcher(out t_watcherComponent watcherComponent) == false)
			{
				Debug.LogError($"Can't register to a {nameof(ISelectionWatcherComponent)} of type {typeof(t_watcherComponent)}");
				return false;
			}

			watcherComponent.Unregister(callback);
			return true;
		}
		#endregion APIs

		#region Privates
		private bool TryGetSelectionWatcher<t_watcherComponent>(out t_watcherComponent selectionWatcherComponent) where t_watcherComponent : ISelectionWatcherComponent
		{
			int selectionWatcherComponentsCount = _selectionWatcherComponents.Length;
			for (int i = 0; i < selectionWatcherComponentsCount; i++)
			{
				selectionWatcherComponent = _selectionWatcherComponents[i] as t_watcherComponent;
				if (selectionWatcherComponent != null)
				{
					return true;
				}
			}

			selectionWatcherComponent = null;
			return false;
		}
		#endregion Privates

		#region Callbacks
		/// <summary>
		/// Called on <see cref="Selection.selectionChanged"/>.
		/// </summary>
		private void OnSelectionChanged()
		{
			Object[] selectedObjects = Selection.objects;

			int selectionWatcherComponentsCount = _selectionWatcherComponents.Length;
			for (int i = 0; i < selectionWatcherComponentsCount; i++)
			{
				_selectionWatcherComponents[i].Evaluate(selectedObjects);
			}
		}
		#endregion Callbacks
		#endregion Methods
	}
}