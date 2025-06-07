namespace MenuGraph.Editor
{
	using System.Collections.Generic;
	using UnityEngine;

	internal sealed class ObjectSelectedComponent : ISelectionWatcherComponent
	{
		#region Constructors
		internal ObjectSelectedComponent() : base()
		{ }
		#endregion Constructors

		#region Methods
		internal override IEnumerable<Object> RetrieveObjects(Object[] selectedObjects)
		{
			int selectedObjectsCount = selectedObjects.Length;
			for (int i = 0; i < selectedObjectsCount; i++)
			{
				yield return selectedObjects[i];
			}
		}
		#endregion Methods
	}
}