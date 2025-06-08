namespace MenuGraph.Editor
{
	using System;
	using System.Collections.Generic;
	using UnityEditor.Experimental.GraphView;

	internal sealed class GraphViewChangesHandler : IDisposable
	{
		#region Delegates
		internal delegate void OnGraphElementRemoved(GraphElement elementToRemove);
		internal delegate void OnEdgeCreated(Edge edgeToCreate);
		#endregion Delegates

		#region Fields
		private GraphView _target = null;
		#endregion Fields

		#region Events
		private OnGraphElementRemoved _graphElementRemoved = null;
		public event OnGraphElementRemoved GraphElementRemoved
		{
			add
			{
				_graphElementRemoved -= value;
				_graphElementRemoved += value;
			}
			remove
			{
				_graphElementRemoved -= value;
			}
		}

		private OnEdgeCreated _edgeCreated = null;
		public event OnEdgeCreated EdgeCreated
		{
			add
			{
				_edgeCreated -= value;
				_edgeCreated += value;
			}
			remove
			{
				_edgeCreated -= value;
			}
		}
		#endregion Events

		#region Constructors
		internal GraphViewChangesHandler(GraphView target)
		{
			_target = target;

			_target.graphViewChanged += OnGraphViewChanged;
		}
		#endregion Constructors

		#region Methods
		#region Lifecycle
		public void Dispose()
		{
			if (_target != null)
			{
				_target.graphViewChanged -= OnGraphViewChanged;
				_target = null;
			}

			_graphElementRemoved = null;
		}
		#endregion Lifecycle

		private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
		{
			HandleElementsToRemove(graphViewChange.elementsToRemove);
			HandleEdgesToCreate(graphViewChange.edgesToCreate);

			return graphViewChange;
		}

		private void HandleElementsToRemove(List<GraphElement> elementsToRemove)
		{
			if (elementsToRemove != null)
			{
				int elementsToRemoveCount = elementsToRemove.Count;
				for (int i = 0; i < elementsToRemoveCount; i++)
				{
					GraphElement elementToRemove = elementsToRemove[i];
					_graphElementRemoved?.Invoke(elementToRemove);
				}
			}
		}
		
		private void HandleEdgesToCreate(List<Edge> edgesToCreate)
		{
			if (edgesToCreate != null)
			{
				int edgesToCreateCount = edgesToCreate.Count;
				for (int i = 0; i < edgesToCreateCount; i++)
				{
					Edge edgeToCreate = edgesToCreate[i];
					_edgeCreated?.Invoke(edgeToCreate);
				}
			}
		}
		#endregion Methods
	}
}