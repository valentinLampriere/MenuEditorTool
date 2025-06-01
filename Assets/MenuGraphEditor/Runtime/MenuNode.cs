namespace MenuGraph
{
	using System;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.InputSystem;
	using UnityEngine.UI;

	public sealed partial class MenuNode : MonoBehaviour
	{
		#region Inner Classes
		[Serializable]
		public sealed class MenuNodeAction
		{
			#region Fields
			[SerializeField] private InputActionAsset _inputActionAsset = null;
			//[SerializeField] private InputAction _inputAction = null;
			[SerializeField] private Button _button = null;
			#endregion Fields

			#region Methods
			public bool IsValid()
			{
				return _inputActionAsset != null || _button != null;
			}
			#endregion Methods
		}
		#endregion Inner Classes

		#region Fields
		[SerializeField] private List<MenuNodeAction> _menuActions = null;
		#endregion Fields

		#region Properties
		public IReadOnlyList<MenuNodeAction> MenuAction { get { return _menuActions; } }
		#endregion Properties
	}
}