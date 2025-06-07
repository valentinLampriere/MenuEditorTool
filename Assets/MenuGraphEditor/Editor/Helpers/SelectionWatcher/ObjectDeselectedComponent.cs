namespace MenuGraph.Editor
{
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	internal sealed class ObjectDeselectedComponent : ISelectionWatcherComponent
	{
		#region Fields
		private List<Object> _previouslySelectedObjects = null;
		#endregion Fields

		#region Constructors
		internal ObjectDeselectedComponent() : base()
		{
			_previouslySelectedObjects = new List<Object>(Selection.objects);
		}
		#endregion Constructors

		#region Methods
		public override void Dispose()
		{
			_previouslySelectedObjects?.Clear();
			_previouslySelectedObjects = null;

			base.Dispose();
		}

		internal override IEnumerable<Object> RetrieveObjects(Object[] selectedObjects)
		{
			int previouslySelectedObjectsCount = _previouslySelectedObjects.Count;
			for (int i = 0; i < previouslySelectedObjectsCount; i++)
			{
				Object previouslySelectedObject = _previouslySelectedObjects[i];

				if (Selection.Contains(previouslySelectedObject) == false)
				{
					yield return previouslySelectedObject;
				}
			}

			_previouslySelectedObjects = new List<Object>(selectedObjects);
		}
		#endregion Methods
	}
}