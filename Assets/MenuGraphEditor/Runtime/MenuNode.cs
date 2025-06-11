namespace MenuGraph
{
	using System.Collections.Generic;
	using UnityEngine;

	public sealed partial class MenuNode : ScriptableObject, ISerializationCallbackReceiver
	{
		#region Fields
		[SerializeField] private MenuUI _targetMenu = null;
		[SerializeField] private MenuNode _parent = null;
		// TODO: Shouldn't be a single list. Better handle children nodes.
		[SerializeField] private List<MenuNode> _children = new List<MenuNode>();
		#endregion Fields

		#region Properties
		// TODO: Instead of adding a setter, maybe it's possible to use Binding to bind the field to a UI element?
		public MenuUI TargetMenu { get { return _targetMenu; } set { _targetMenu = value; } }
		public MenuNode Parent { get { return _parent; } set { _parent = value; } }
		public List<MenuNode> Children { get { return _children; } set { _children = value; } }
		#endregion Properties

		#region Methods
		public bool TryGetNextMenuNode(int index, out MenuNode nextMenuNode)
		{
			if (_children == null)
			{
				nextMenuNode = null;
				return false;
			}

			if (index < 0 || index >= _children.Count)
			{
				nextMenuNode = null;
				return false;
			}

			nextMenuNode = _children[index];
			return nextMenuNode != null;
		}

		#region ISerializationCallbackReceiver
		public void OnBeforeSerialize()
		{
			int menuActionsCount = _targetMenu != null ? _targetMenu.MenuActions.Count : 0;
			int childrenCount = _children.Count;

			if (menuActionsCount > childrenCount)
			{
				for (int i = childrenCount; i < menuActionsCount; i++)
				{
					_children.Add(null);
				}

				Debug.Log($"Missing {menuActionsCount - childrenCount} children in this {nameof(MenuNode)} ({name}). Automatically add required children.");
			}
			else if (childrenCount > menuActionsCount)
			{
				Debug.LogWarning($"There are no many children for this {nameof(MenuNode)} ({name}). {childrenCount} registered while {menuActionsCount} expected.");
			}
		}

		public void OnAfterDeserialize()
		{

		}
		#endregion ISerializationCallbackReceiver
		#endregion Methods
	}
}